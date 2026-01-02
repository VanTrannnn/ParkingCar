using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject panelLevel;
    [SerializeField] GameObject menu;
    private GameObject[] levelButton;
    private void Awake()
    {
        menu.SetActive(true);
        int childCount = panelLevel.transform.childCount;
        levelButton = new GameObject[childCount];
        for(int i=0; i<childCount; i++)
        {
            levelButton[i] = panelLevel.transform.GetChild(i).gameObject;
            levelButton[i].SetActive(false);

            //add event onclick de load level 
            int sceneIndex = i + 1;
            levelButton[i].GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneIndex));
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
    }
    public void SelectLevel() {
        Debug.Log("clicked");
        menu.SetActive(false);
        panelLevel.SetActive(true);
        for (int i = 0; i < PlayerPrefs.GetInt("highestLevel", 1); i++)
        {
            levelButton[i].SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    // Update is called once per frame
    void Update()
    {
        // Kiểm tra nếu panelLevel đang active và người dùng nhấn ESC
        if (panelLevel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseLevelPanel();
        }
    }
    
    private void CloseLevelPanel()
    {
        panelLevel.SetActive(false);
        menu.SetActive(true);
    }
}
