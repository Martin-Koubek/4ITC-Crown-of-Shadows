using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class CorridorGenerator : MonoBehaviour
{
    [Header("Physics Nastavení")]
    public LayerMask vrstvaMistnosti; // V inspectoru nastavte na layer "Room"

    [Header("Nastavení objektù")]
    public GameObject floorPrefab; // Váš 3D model podlahy (kostka/dlaždice)
    public Transform corridorParent; // Prázdný objekt v Hierarchy pro poøádek

    [Header("Nastavení møížky")]
    public Grid grid; // Reference na Grid komponentu ve scénì
    [Range(0, 5)] public int polomer = 3; // Polomìr 3 = šíøka 7 dlaždic

    // HashSet hlídá, abychom na jedno místo nepoložili dvì kostky
    private HashSet<Vector3Int> obsazeno = new HashSet<Vector3Int>();

    // HLAVNÍ FUNKCE, KTEROU VOLÁTE ZE SVÉHO GENERÁTORU
    public void PropojMistnosti(List<Room> vsechnyMistnosti)
    {
        if (vsechnyMistnosti == null || vsechnyMistnosti.Count < 2) return;

        // Vyèistíme scénu, pokud už tam nìjaké chodby byly
        OcistiMapu();

        // 1. Získáme všechny vchody (ConnectionPoints)
        List<ConnectionPoint> vsechnyBody = new List<ConnectionPoint>();
        foreach (var room in vsechnyMistnosti)
        {
            vsechnyBody.AddRange(room.connectionPoints);
        }

        // 2. Najdeme nejkratší logické propojení (MST)
        List<Hrana> mstHrany = NajdiMST(vsechnyBody);

        // 3. Fyzicky vytvoøíme 3D objekty ve scénì
        VykresliChodby(mstHrany);
    }

    // --- LOGICKÁ ÈÁST: KRUSKALÙV ALGORITMUS ---
    private List<Hrana> NajdiMST(List<ConnectionPoint> body)
    {
        List<Hrana> mozneHrany = new List<Hrana>();

        // Vytvoøíme všechny možné kombinace spojení (místnost A -> místnost B)
        for (int i = 0; i < body.Count; i++)
        {
            for (int j = i + 1; j < body.Count; j++)
            {
                // Spojujeme jen vchody, které nepatøí stejné místnosti
                if (body[i].GetComponentInParent<Room>() != body[j].GetComponentInParent<Room>())
                {
                    mozneHrany.Add(new Hrana(body[i], body[j]));
                }
            }
        }

        // Seøadíme od nejkratších spojù
        var serazeneHrany = mozneHrany.OrderBy(h => h.vaha).ToList();
        List<Hrana> vysledek = new List<Hrana>();

        // Union-Find pro detekci cyklù
        Dictionary<ConnectionPoint, ConnectionPoint> rodice = body.ToDictionary(b => b, b => b);

        ConnectionPoint NajdiRodice(ConnectionPoint b)
        {
            if (rodice[b] == b) return b;
            return rodice[b] = NajdiRodice(rodice[b]);
        }

        foreach (var hrana in serazeneHrany)
        {
            ConnectionPoint r1 = NajdiRodice(hrana.cp1);
            ConnectionPoint r2 = NajdiRodice(hrana.cp2);

            if (r1 != r2)
            {
                vysledek.Add(hrana);
                rodice[r1] = r2; // Spojíme skupiny
            }
        }
        return vysledek;
    }

    // --- FYZICKÁ ÈÁST: INSTANTIATE ---
    private void VykresliChodby(List<Hrana> hrany)
    {
        foreach (var hrana in hrany)
        {
            Vector3Int start = grid.WorldToCell(hrana.cp1.transform.position);
            Vector3Int konec = grid.WorldToCell(hrana.cp2.transform.position);

            // Roh L-chodby
            Vector2Int roh = new Vector2Int(konec.x, start.z);

            // Kreslíme dvì samostatná ramena, ne jeden velký ètverec!
            // 1. Horizontální pruh (osa X)
            VykresliPruh(start.x, konec.x, start.z, start.z, start.y);

            // 2. Vertikální pruh (osa Z) - zaèínáme v rohu
            VykresliPruh(konec.x, konec.x, start.z, konec.z, start.y);
        }
    }



    private void VykresliPruh(int xOd, int xDo, int zOd, int zDo, int y)
    {
        int minX = Mathf.Min(xOd, xDo);
        int maxX = Mathf.Max(xOd, xDo);
        int minZ = Mathf.Min(zOd, zDo);
        int maxZ = Mathf.Max(zOd, zDo);

        for (int x = minX - polomer; x <= maxX + polomer; x++)
        {
            for (int z = minZ - polomer; z <= maxZ + polomer; z++)
            {
                Vector3Int pos = new Vector3Int(x, y, z);

                if (!obsazeno.Contains(pos))
                {
                    Vector3 worldPos = grid.GetCellCenterWorld(pos);

                    // --- KLÍÈOVÁ ZMÌNA ---
                    // Zkontrolujeme, zda v tomto místì není collider místnosti
                    // Velikost sféry (0.4f) by mìla být o nìco menší než polovina cell size
                    if (!Physics.CheckSphere(worldPos, 0.4f, vrstvaMistnosti))
                    {
                        Instantiate(floorPrefab, worldPos, Quaternion.identity, corridorParent);
                        obsazeno.Add(pos);
                    }
                    else
                    {
                        // Pokud narazíme na místnost, dlaždici nepoložíme, 
                        // ale pozici si oznaèíme jako obsazenou, aby se tam už nic nezkoušelo
                        obsazeno.Add(pos);
                    }
                }
            }
        }
    }

    public void OcistiMapu()
    {
        obsazeno.Clear();
        if (corridorParent == null) return;

        // Smaže všechny staré objekty chodeb
        foreach (Transform child in corridorParent)
        {
            Destroy(child.gameObject);
        }
    }
}