using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {

    [SerializeField] Text bestScoreText;
    [SerializeField] InputField nameField;

    public string name = "";

    public string bestName;
    public int bestScore;

    public static GameManager Instance;

    private void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGameInfo();
    }

    private void Start() {
        
    }

    public void SetBestScore(int score) {
        if(score > bestScore) {
            bestScore = score;
            bestName = name;
            SaveGameInfo();
            MainManager.Instance.BestScoreText.text = "melhor pontuação : " + bestName + " : " + bestScore;   // substituir a melhor pontuação e o melhor nome
        }
        Debug.Log("Pontuação: " + score + "  Jogador: " + name);
    }

    public void StartNew() {
        if(nameField.text != "") {
            name = nameField.text;
            SceneManager.LoadScene(1);
        } else {
          Debug.LogWarning("METE O TEU NOME BOT");           //Para escrever o nome, caso não metas o nome, avisa-te para meter o nome
        }
    }

    public void Exit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else                                                         //sair
    Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData {
        public string name;
        public int bestScore;
    }

    public void SaveGameInfo() {
        SaveData data = new SaveData();
        data.name = bestName;
        data.bestScore = bestScore;                                 //salvar a informação 

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameInfo() {
        string path = Application.persistentDataPath + "/savefile.json"; //dar load a informação

        if(File.Exists(path)) {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.name;
            bestScore = data.bestScore;
        }
    }
}
