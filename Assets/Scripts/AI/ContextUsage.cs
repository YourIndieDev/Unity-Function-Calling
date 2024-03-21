using Danejw;
using UnityEngine;


namespace AI
{
    public class ContextUsage : MonoBehaviour
    {
        public Context<bool> isActive = new Context<bool>("Active", true);
        public Context<int> lives = new Context<int>("Lives", 1);
        public Context<float> position = new Context<float>("Position", 2);
        public Context<string> name = new Context<string>("Name", "Dane");

        [Button("Set Random Active")]
        public void SetActive()
        {
            isActive.SetValue(UnityEngine.Random.Range(0, 2) > 0); // Randomly sets to true or false
        }

        [Button("Set Random Lives")]
        public void SetLives()
        {
            lives.SetValue(UnityEngine.Random.Range(1, 10)); // Randomly sets lives between 1 and 10
        }

        [Button("Set Random Position")]
        public void SetPosition()
        {
            position.SetValue(UnityEngine.Random.Range(-10f, 10f)); // Randomly sets position between -10 and 10
        }

        [Button("Set Random Name")]
        public void SetName()
        {
            name.SetValue("Name" + UnityEngine.Random.Range(1, 100).ToString()); // Sets a random name
        }



        public struct Data
        {
            public int id;
            public string name;
            public string description;
            public float x;
            public float y;
        }

        public Context<Data> data = new Context<Data>("Data", new Data());
        [Button("Change Data")]
        public void ChangeData()
        {
            var newData = new Data
            {
                id = Random.Range(1, 100),
                x = Random.Range(-10f, 10f),
                y = Random.Range(-10f, 10f),
                name = Random.Range(1, 100).ToString(),
                description = Random.Range(1, 100).ToString()
            };
            data.SetValue(newData);
        }

        public Context<Data> data2 = new Context<Data>("Data2", new Data());
        [Button("Change Data 2")]
        public void ChangeData2()
        {
            var newData = new Data
            {
                id = Random.Range(1, 100),
                x = Random.Range(-10f, 10f),
                y = Random.Range(-10f, 10f),
                name = Random.Range(1, 100).ToString(),
                description = Random.Range(1, 100).ToString()
            };
            data2.SetValue(newData);
        }


        // Hypothetical Game Situation
        #region Hypothetical Game Situation
        // Player data
        public struct PlayerData
        {
            public string name;
            public int health;
            public int ammo;
            public int kills;
            public int assists;
        }

        // Match information
        public struct MatchData
        {
            public string mapName;
            public float matchDuration;
            public string matchStatus; // e.g., "In Progress", "Finished"
        }

        // Team data
        public struct TeamData
        {
            public string teamName;
            public int totalScore;
            public string[] playerNames;
        }

        // Game state
        public struct GameState
        {
            public MatchData currentMatch;
            public TeamData teamA;
            public TeamData teamB;
            public string currentLeader;
        }

        public Context<PlayerData> player1DataContext = new Context<PlayerData>("Player1", new PlayerData());
        public Context<PlayerData> player2DataContext = new Context<PlayerData>("Player2", new PlayerData());
        public Context<PlayerData> player3DataContext = new Context<PlayerData>("Player3", new PlayerData());
        public Context<PlayerData> player4DataContext = new Context<PlayerData>("Player4", new PlayerData());
        public Context<MatchData> matchDataContext = new Context<MatchData>("Match", new MatchData());
        public Context<TeamData> teamDataContext = new Context<TeamData>("Team", new TeamData());
        public Context<GameState> gameStateContext = new Context<GameState>("GameState", new GameState());

        public void Start()
        {
            //CreateRandomypotheticalGameDatta();
        }

        [Button("Create Random Hypothetical GameData")]
        private void CreateRandomypotheticalGameDatta()
        {
            player1DataContext.SetValue(CreateRandomPlayerData());
            player2DataContext.SetValue(CreateRandomPlayerData());
            player3DataContext.SetValue(CreateRandomPlayerData());
            player4DataContext.SetValue(CreateRandomPlayerData());
            matchDataContext.SetValue(CreateRandomMatchData());
            teamDataContext.SetValue(CreateRandomTeamData());
            gameStateContext.SetValue(CreateRandomGameState());
        }

        [Button("CreateRandomPlayerData")]
        public PlayerData CreateRandomPlayerData()
        {
            return new PlayerData
            {
                name = $"Player {UnityEngine.Random.Range(1, 100)}",
                health = UnityEngine.Random.Range(1, 100),
                ammo = UnityEngine.Random.Range(0, 50),
                kills = UnityEngine.Random.Range(0, 10),
                assists = UnityEngine.Random.Range(0, 10)
            };
        }
        [Button("CreateRandomMatchData")]
        public MatchData CreateRandomMatchData()
        {
            string[] mapNames = new string[] { "Map1", "Map2", "Map3" };
            return new MatchData
            {
                mapName = mapNames[UnityEngine.Random.Range(0, mapNames.Length)],
                matchDuration = UnityEngine.Random.Range(5f, 60f),
                matchStatus = UnityEngine.Random.Range(0, 2) > 0 ? "In Progress" : "Finished"
            };
        }
        [Button("CreateRandomTeamData")]
        public TeamData CreateRandomTeamData()
        {
            return new TeamData
            {
                teamName = "Team" + UnityEngine.Random.Range(1, 10),
                totalScore = UnityEngine.Random.Range(0, 100),
                playerNames = new string[] { "PlayerA", "PlayerB", "PlayerC" } // Example names
            };
        }
        [Button("CreateRandomGameState")]
        public GameState CreateRandomGameState()
        {
            return new GameState
            {
                currentMatch = CreateRandomMatchData(),
                teamA = CreateRandomTeamData(),
                teamB = CreateRandomTeamData(),
                currentLeader = UnityEngine.Random.Range(0, 2) > 0 ? "TeamA" : "TeamB"
            };
        }
        #endregion

        // Storing multiple contexts which will give a sense of change over time
        [Button("Add Context Over Time")]
        public void AddContextOverTime()
        {
            ContextManager.AddContextsOverTime(System.DateTime.Now.ToString());
        }

        public class ContextData
        {
            public Context<bool> isActive = new Context<bool>("Active", true);
            public Context<int> lives = new Context<int>("Lives", 1);
            public Context<float> position = new Context<float>("Position", 2);
            public Context<string> name = new Context<string>("Name", "Dane");
        }


        // Increasing complexity to having a ContextDatas with a ContextDatas
        public Context<ContextData> dataOfContextData = new Context<ContextData>("DataOfContextData", new ContextData());

        [Button("Generate ContextData of Context Data")]
        public void GenerateContextData()
        {
            var data = new ContextData();
            data.isActive.SetValue(UnityEngine.Random.Range(0, 2) > 0);
            data.lives.SetValue(UnityEngine.Random.Range(1, 10));
            data.position.SetValue(UnityEngine.Random.Range(-10f, 10f));
            data.name.SetValue("Name" + UnityEngine.Random.Range(1, 100).ToString());
            dataOfContextData.SetValue(data);
        }

        [Button("Generate Data Of ContextData")]
        public void GenerateDataOfContextData()
        {
            dataOfContextData.Value.isActive.SetValue(UnityEngine.Random.Range(0, 2) > 0);
            dataOfContextData.Value.lives.SetValue(UnityEngine.Random.Range(1, 10));
            dataOfContextData.Value.position.SetValue(UnityEngine.Random.Range(-10f, 10f));
            dataOfContextData.Value.name.SetValue("Name" + UnityEngine.Random.Range(1, 100).ToString());
        }


        // Serialize to jsons objects
        [Button("Serialize Context")]
        public void SerializeContext()
        {
            Debug.Log(ContextManager.SerializeContextJson());
        }


        [Button("Serialize Context OverTime")]
        public void SerializeContextOvertime()
        {
            Debug.Log(ContextManager.SerializeContextOverTimeJson());
        }

        [Button("Clear Context")]
        public void ClearContext()
        {
            ContextManager.ClearContextJson();

            Debug.Log("Clearing AI Context.");
        }
    }
}
