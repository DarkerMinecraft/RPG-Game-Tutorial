using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Mover mover;
    private new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        camera = Camera.main;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        RaycastHit hit;


        Ray ray = camera.ScreenPointToRay(mousePos);
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            mover.MoveTo(hit.point);
        }
    }


}
