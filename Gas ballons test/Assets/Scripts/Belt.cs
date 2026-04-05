using UnityEngine;
using UnityEngine.Events;

public class Belt : MonoBehaviour
{
    [SerializeField] private float Height;
    [SerializeField] private Transform Head;

    public UnityEvent OnResetBeltItems;

    public void ResetBeltItems()
    {
        OnResetBeltItems.Invoke();
    }

    public void DeactivateBelt()
    {
        gameObject.SetActive(false);
    }

    public void ActivateBelt()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        DeactivateBelt();
    }

    void Update()
    {
        Vector3 currentPos = new Vector3(Head.position.x, Head.position.y - Height, Head.position.z);

        transform.position = currentPos;
        transform.rotation = Quaternion.Euler(0f, Head.rotation.eulerAngles.y, 0f);
    }
}
