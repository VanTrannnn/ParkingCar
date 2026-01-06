using UnityEngine;

public class ClosePanelTutorial : MonoBehaviour
{
    [SerializeField] GameObject panel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        panel.SetActive(false);
    }
}
