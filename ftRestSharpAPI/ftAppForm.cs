//FT: A Football Info REST C# App
//by Ben Lawlor

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json;
//using DragAssembly;

namespace ftRestSharpAPI
{
    public partial class ftAppForm : Form
    {
        //public IRestResponse premierLeague;
        public string[][] premierLeagueTable = new string[20][];
        public string[][] ligue1Table = new string[20][];
        public string[][] bundesligaTable = new string[18][];
        public string[][] laLigaTable = new string[20][];
        public string[][] serieATable = new string[20][];

        public string[][] championsLeagueGroupATable = new string[4][];
        public string[][] championsLeagueGroupBTable = new string[4][];
        public string[][] championsLeagueGroupCTable = new string[4][];
        public string[][] championsLeagueGroupDTable = new string[4][];
        public string[][] championsLeagueGroupETable = new string[4][];
        public string[][] championsLeagueGroupFTable = new string[4][];
        public string[][] championsLeagueGroupGTable = new string[4][];
        public string[][] championsLeagueGroupHTable = new string[4][];

        public string[][] premierLeagueResults = new string[20][];
        public string[][] bundesligaResults = new string[20][];
        public string[][] laLigaResults = new string[20][];
        public string[][] serieAResults = new string[20][];
        public string[][] ligue1Results = new string[20][];
        public string[][] championsLeagueResults = new string[20][];

        public string[][] premierLeagueFixtures = new string[30][];
        public string[][] bundesligaFixtures = new string[30][];
        public string[][] laLigaFixtures = new string[30][];
        public string[][] serieAFixtures = new string[30][];
        public string[][] ligue1Fixtures = new string[30][];
        public string[][] championsLeagueFixtures = new string[30][];

        public string topScorer;
        public string storeTopScorer;
        public string topScorerID;
        public string topScorerImageUrl;
        public string competitionID;
        public string sevenDaysAgo = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");

        //Form premierLeagueForm;

        public ftAppForm()
        {
            InitializeComponent();

            //Executing all league table Rest Requests 
            //and all Champions League group table Rest Requests
            //when the application launches
            //and adding them to string arrays.
            //This keeps API calls to a minimum 
            //and lets our app access the data from the response much faster.
            //The refresh button performs new requests to update.

            //These requests all follow the same format:
            //Performing a Rest Request from a Rest Client,
            //Getting a Rest Response,
            //Deserializing the Rest Response,
            //Extracting the relevant deserialized data
            //And adding that data to its specific string array.

            //For the various basic league tables we are using 
            //Heisenbugs Live Scores APIs from RapidAPI.com


            //PREMIER LEAGUE 
            var premierLeagueTableClient = new RestClient("https://heisenbug-premier-league-live-scores-v1.p.rapidapi.com/api/premierleague/table");
            var premierLeagueTableRequest = new RestRequest(Method.GET);
            premierLeagueTableRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            premierLeagueTableRequest.AddHeader("x-rapidapi-host", "heisenbug-premier-league-live-scores-v1.p.rapidapi.com");
            IRestResponse premierLeagueTableResponse = premierLeagueTableClient.Execute(premierLeagueTableRequest);

            //The rest response is then deserialzed 
            var premierLeagueTableDeserialized = JsonConvert.DeserializeObject<LeagueTableRootObject>(premierLeagueTableResponse.Content);

            //Heisenbugs APIs do not specify a teams goal difference 
            //or their position in the table, so
            //they are designed here and added to each team's array.
            //These requests will be repeated for other leagues

            int arrayIndex = 0;
            foreach (Record a in premierLeagueTableDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                //The parameters of the Rest Response are added to the 2D Premier League Table string array
                //The integer variable called arrayIndex is used to index each team's respective information in the 2d array
                //The full 2d array is then later added to the league table list view
                premierLeagueTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), a.goalsFor.ToString(), a.goalsAgainst.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }


            //BUNDESLIGA
            var bundesligaClient = new RestClient("https://heisenbug-bundesliga-live-scores-v1.p.rapidapi.com/api/bundesliga/table");
            var bundesligaRequest = new RestRequest(Method.GET);
            bundesligaRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            bundesligaRequest.AddHeader("x-rapidapi-host", "heisenbug-bundesliga-live-scores-v1.p.rapidapi.com");
            IRestResponse bundesligaTableResponse = bundesligaClient.Execute(bundesligaRequest);

            //The rest response is deserialzed here:
            var bundesligaTableDeserialized = JsonConvert.DeserializeObject<LeagueTableRootObject>(bundesligaTableResponse.Content);

            //Heisenbugs APIs do not specify a teams goal difference 
            //or their position in the table, so
            //they are designed here and added to each team's array

            arrayIndex = 0;
            foreach (Record a in bundesligaTableDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                //The parameters of the Rest Response are added to the 2D Premier League Table string array
                //The arrayIndex variable is used to store each team's respective information in the 2d array
                //The full 2d array is then later added to the league table list view

                bundesligaTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), a.goalsFor.ToString(), a.goalsAgainst.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }


            //LA LIGA
            var laLigaClient = new RestClient("https://heisenbug-la-liga-live-scores-v1.p.rapidapi.com/api/laliga/table");
            var laLigaRequest = new RestRequest(Method.GET);
            laLigaRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            laLigaRequest.AddHeader("x-rapidapi-host", "heisenbug-la-liga-live-scores-v1.p.rapidapi.com");
            IRestResponse laLigaResponse = laLigaClient.Execute(laLigaRequest);

            //Deserializing the Rest Response
            var laLigaTableDeserialized = JsonConvert.DeserializeObject<LeagueTableRootObject>(laLigaResponse.Content);

            //Heisenbugs APIs do not specify a teams goal difference 
            //or their position in the table, so
            //they are designed here and added to each team's array

            arrayIndex = 0;
            foreach (Record a in laLigaTableDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();
                else
                    goalDifference = gD.ToString();

                //The parameters of the Rest Response are added to the 2D Premier League Table string array
                //The arrayIndex variable is used to store each team's respective information in the 2d array
                //The full 2d array is then later added to the league table list view

                laLigaTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), a.goalsFor.ToString(), a.goalsAgainst.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }


            //LIGUE 1
            var ligue1Client = new RestClient("https://heisenbug-ligue-1-live-scores-v1.p.rapidapi.com/api/ligue1/table");
            var ligue1Request = new RestRequest(Method.GET);
            ligue1Request.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            ligue1Request.AddHeader("x-rapidapi-host", "heisenbug-ligue-1-live-scores-v1.p.rapidapi.com");
            IRestResponse ligue1TableResponse = ligue1Client.Execute(ligue1Request);

            var ligue1TableDeserialized = JsonConvert.DeserializeObject<LeagueTableRootObject>(ligue1TableResponse.Content);

            arrayIndex = 0;
            foreach (Record a in ligue1TableDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                ligue1Table[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), a.goalsFor.ToString(), a.goalsAgainst.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }


            //SERIE A            
            var serieAClient = new RestClient("https://heisenbug-seriea-live-scores-v1.p.rapidapi.com/api/serie-a/table");
            var serieARequest = new RestRequest(Method.GET);
            serieARequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            serieARequest.AddHeader("x-rapidapi-host", "heisenbug-seriea-live-scores-v1.p.rapidapi.com");
            IRestResponse serieAResponse = serieAClient.Execute(serieARequest);

            var serieATableDeserialized = JsonConvert.DeserializeObject<LeagueTableRootObject>(serieAResponse.Content);

            arrayIndex = 0;
            foreach (Record a in serieATableDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                serieATable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), a.goalsFor.ToString(), a.goalsAgainst.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //CHAMPIONS LEAGUE GROUPS
            //These Champions League group sections perform the same procedure as with the leagues before:
            //Performing a Rest Request from a Rest Client,
            //Getting a Rest Response,
            //Deserializing the Rest Response,
            //Extracting the relevant deserialized data
            //And adding that data to its specific string array
            //To minimize the need for requests and to make the app perform faster

            //GROUP A
            var cLGroupAClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=A");
            var cLGroupARequest = new RestRequest(Method.GET);
            cLGroupARequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupARequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            
            IRestResponse cLGroupAResponse = cLGroupAClient.Execute(cLGroupARequest);

            var groupADeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupAResponse.Content);

            arrayIndex = 0;
            foreach(CLGroupRecord a in groupADeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupATable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP B
            var cLGroupBClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=B");
            var cLGroupBRequest = new RestRequest(Method.GET);
            cLGroupBRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupBRequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupBResponse = cLGroupBClient.Execute(cLGroupBRequest);

            var groupBDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupBResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupBDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupBTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP C
            var cLGroupCClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=C");
            var cLGroupCRequest = new RestRequest(Method.GET);
            cLGroupCRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupCRequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupCResponse = cLGroupCClient.Execute(cLGroupCRequest);

            var groupCDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupCResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupCDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupCTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP D
            var cLGroupDClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=D");
            var cLGroupDRequest = new RestRequest(Method.GET);
            cLGroupDRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupDRequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupDResponse = cLGroupDClient.Execute(cLGroupDRequest);

            var groupDDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupDResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupDDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupDTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP E
            var cLGroupEClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=E");
            var cLGroupERequest = new RestRequest(Method.GET);
            cLGroupERequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupERequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupEResponse = cLGroupEClient.Execute(cLGroupERequest);

            var groupEDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupEResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupEDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupETable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP F
            var cLGroupFClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=F");
            var cLGroupFRequest = new RestRequest(Method.GET);
            cLGroupFRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupFRequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupFResponse = cLGroupFClient.Execute(cLGroupFRequest);

            var groupFDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupFResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupFDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupFTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP G
            var cLGroupGClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=G");
            var cLGroupGRequest = new RestRequest(Method.GET);
            cLGroupGRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupGRequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupGResponse = cLGroupGClient.Execute(cLGroupGRequest);

            var groupGDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupGResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupGDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupGTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }

            //GROUP H
            var cLGroupHClient = new RestClient("https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=H");
            var cLGroupHRequest = new RestRequest(Method.GET);
            cLGroupHRequest.AddHeader("x-rapidapi-key", "ENTER_RAPID_API_KEY_HERE");
            cLGroupHRequest.AddHeader("x-rapidapi-host", "heisenbug-champions-league-live-scores-v1.p.rapidapi.com");
            IRestResponse cLGroupHResponse = cLGroupHClient.Execute(cLGroupHRequest);

            var groupHDeserialized = JsonConvert.DeserializeObject<ChampionsLeagueGroupRootobject>(cLGroupHResponse.Content);

            arrayIndex = 0;
            foreach (CLGroupRecord a in groupHDeserialized.records)
            {
                int tablePosition = arrayIndex + 1;
                int gD = a.goalsFor - a.goalsAgainst;
                string goalDifference;

                if (gD >= 0)
                    goalDifference = "+" + gD.ToString();

                else
                    goalDifference = gD.ToString();

                championsLeagueGroupHTable[arrayIndex] = new string[] { tablePosition.ToString(), a.team, a.played.ToString(), a.win.ToString(), a.draw.ToString(), a.loss.ToString(), goalDifference, a.points.ToString() };

                arrayIndex++;
            }



            //PREMIER LEAGUE RESULTS REQUEST
            //
            var premScoresClient = new RestClient("http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var premScoresRequest = new RestRequest(Method.GET);
            premScoresRequest.AddParameter("competition_id", "2");
            premScoresRequest.AddParameter("from", sevenDaysAgo);

            IRestResponse premScoresResponse = premScoresClient.Execute(premScoresRequest);

            var premScoresResponseDeserialized = JsonConvert.DeserializeObject<ResultsRootobject>(premScoresResponse.Content);

            arrayIndex = 0;
            foreach (ResultsMatch a in premScoresResponseDeserialized.data.match)
            {
                premierLeagueResults[arrayIndex] = new string[] { a.home_name, a.score, a.away_name, a.date };
                arrayIndex++;
            }
            //

            //PREMIER LEAGUE FIXTURES REQUEST
            //
            var premFixturesClient = new RestClient("https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var premFixturesRequest = new RestRequest(Method.GET);
            premFixturesRequest.AddParameter("competition_id", "2");
            IRestResponse premFixturesResponse = premFixturesClient.Execute(premFixturesRequest);

            var premFixturesResponseDeserialized = JsonConvert.DeserializeObject<FixturesRootobject>(premFixturesResponse.Content);

            arrayIndex = 0;
            foreach (Fixture a in premFixturesResponseDeserialized.data.fixtures)
            {
                premierLeagueFixtures[arrayIndex] = new string[] { a.home_name, " v ", a.away_name, a.date };
                arrayIndex++;
            }
            //

            //BUNDESLIGA RESULTS REQUEST
            //
            var bundesligaScoresClient = new RestClient("http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var bundesligaScoresRequest = new RestRequest(Method.GET);
            bundesligaScoresRequest.AddParameter("competition_id", "1");
            bundesligaScoresRequest.AddParameter("from", sevenDaysAgo);

            IRestResponse bundesligaScoresResponse = bundesligaScoresClient.Execute(bundesligaScoresRequest);

            var bundesligaScoresResponseDeserialized = JsonConvert.DeserializeObject<ResultsRootobject>(bundesligaScoresResponse.Content);

            arrayIndex = 0;
            foreach (ResultsMatch a in bundesligaScoresResponseDeserialized.data.match)
            {
                bundesligaResults[arrayIndex] = new string[] { a.home_name, a.score, a.away_name, a.date };
                arrayIndex++;
            }
            //

            //BUNDESLIGA FIXTURES REQUEST
            //
            var bundesligaFixturesClient = new RestClient("https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var bundesligaFixturesRequest = new RestRequest(Method.GET);
            bundesligaFixturesRequest.AddParameter("competition_id", "1");
            IRestResponse bundesligaFixturesResponse = bundesligaFixturesClient.Execute(bundesligaFixturesRequest);

            var bundesligaFixturesResponseDeserialized = JsonConvert.DeserializeObject<FixturesRootobject>(bundesligaFixturesResponse.Content);

            arrayIndex = 0;
            foreach (Fixture a in bundesligaFixturesResponseDeserialized.data.fixtures)
            {
                bundesligaFixtures[arrayIndex] = new string[] { a.home_name, " v ", a.away_name, a.date };
                arrayIndex++;
            }
            //

            //LA LIGA RESULTS REQUEST
            //
            var laLigaScoresClient = new RestClient("http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var laLigaScoresRequest = new RestRequest(Method.GET);
            laLigaScoresRequest.AddParameter("competition_id", "3");
            laLigaScoresRequest.AddParameter("from", sevenDaysAgo);

            IRestResponse laLigaScoresResponse = laLigaScoresClient.Execute(laLigaScoresRequest);

            var laLigaScoresResponseDeserialized = JsonConvert.DeserializeObject<ResultsRootobject>(laLigaScoresResponse.Content);

            arrayIndex = 0;
            foreach (ResultsMatch a in laLigaScoresResponseDeserialized.data.match)
            {
                laLigaResults[arrayIndex] = new string[] { a.home_name, a.score, a.away_name, a.date };
                arrayIndex++;
            }
            //

            //LA LIGA FIXTURES REQUEST
            //
            var laLigaFixturesClient = new RestClient("https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var laLigaFixturesRequest = new RestRequest(Method.GET);
            laLigaFixturesRequest.AddParameter("competition_id", "3");
            IRestResponse laLigaFixturesResponse = laLigaFixturesClient.Execute(laLigaFixturesRequest);

            var laLigaFixturesResponseDeserialized = JsonConvert.DeserializeObject<FixturesRootobject>(laLigaFixturesResponse.Content);

            arrayIndex = 0;
            foreach (Fixture a in laLigaFixturesResponseDeserialized.data.fixtures)
            {
                laLigaFixtures[arrayIndex] = new string[] { a.home_name, " v ", a.away_name, a.date };
                arrayIndex++;
            }
            //

            //SERIE A RESULTS REQUEST
            //
            var serieAScoresClient = new RestClient("http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var serieAScoresRequest = new RestRequest(Method.GET);
            serieAScoresRequest.AddParameter("competition_id", "4");
            serieAScoresRequest.AddParameter("from", sevenDaysAgo);

            IRestResponse serieAScoresResponse = serieAScoresClient.Execute(serieAScoresRequest);

            var serieAScoresResponseDeserialized = JsonConvert.DeserializeObject<ResultsRootobject>(serieAScoresResponse.Content);

            arrayIndex = 0;
            foreach (ResultsMatch a in serieAScoresResponseDeserialized.data.match)
            {
                serieAResults[arrayIndex] = new string[] { a.home_name, a.score, a.away_name, a.date };
                arrayIndex++;
            }
            //

            //SERIE A FIXTURES REQUEST
            //
            var serieAFixturesClient = new RestClient("https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var serieAFixturesRequest = new RestRequest(Method.GET);
            serieAFixturesRequest.AddParameter("competition_id", "4");
            IRestResponse serieAFixturesResponse = serieAFixturesClient.Execute(serieAFixturesRequest);

            var serieAFixturesResponseDeserialized = JsonConvert.DeserializeObject<FixturesRootobject>(serieAFixturesResponse.Content);

            arrayIndex = 0;
            foreach (Fixture a in serieAFixturesResponseDeserialized.data.fixtures)
            {
                serieAFixtures[arrayIndex] = new string[] { a.home_name, " v ", a.away_name, a.date };
                arrayIndex++;
            }
            //

            //LIGUE 1 RESULTS REQUEST
            //
            var ligue1ScoresClient = new RestClient("http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var ligue1ScoresRequest = new RestRequest(Method.GET);
            ligue1ScoresRequest.AddParameter("competition_id", "5");
            ligue1ScoresRequest.AddParameter("from", sevenDaysAgo);

            IRestResponse ligue1ScoresResponse = ligue1ScoresClient.Execute(ligue1ScoresRequest);

            var ligue1ScoresResponseDeserialized = JsonConvert.DeserializeObject<ResultsRootobject>(ligue1ScoresResponse.Content);

            arrayIndex = 0;
            foreach (ResultsMatch a in ligue1ScoresResponseDeserialized.data.match)
            {
                ligue1Results[arrayIndex] = new string[] { a.home_name, a.score, a.away_name, a.date };
                arrayIndex++;
            }
            //

            //LIGUE 1 FIXTURES REQUEST
            //
            var ligue1FixturesClient = new RestClient("https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var ligue1FixturesRequest = new RestRequest(Method.GET);
            ligue1FixturesRequest.AddParameter("competition_id", "5");
            IRestResponse ligue1FixturesResponse = ligue1FixturesClient.Execute(ligue1FixturesRequest);

            var ligue1FixturesResponseDeserialized = JsonConvert.DeserializeObject<FixturesRootobject>(ligue1FixturesResponse.Content);

            arrayIndex = 0;
            foreach (Fixture a in ligue1FixturesResponseDeserialized.data.fixtures)
            {
                ligue1Fixtures[arrayIndex] = new string[] { a.home_name, " v ", a.away_name, a.date };
                arrayIndex++;
            }
            //

            //CHAMPIONS LEAGUE RESULTS REQUEST
            //
            var cLScoresClient = new RestClient("http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var cLScoresRequest = new RestRequest(Method.GET);
            cLScoresRequest.AddParameter("competition_id", "244");
            cLScoresRequest.AddParameter("from", "2021-01-01");

            IRestResponse cLScoresResponse = cLScoresClient.Execute(cLScoresRequest);

            var cLScoresResponseDeserialized = JsonConvert.DeserializeObject<ResultsRootobject>(cLScoresResponse.Content);

            arrayIndex = 0;
            foreach (ResultsMatch a in cLScoresResponseDeserialized.data.match)
            {
                championsLeagueResults[arrayIndex] = new string[] { a.home_name, a.score, a.away_name, a.date };
                arrayIndex++;
            }
        //


            //CHAMPIONS LEAGUE FIXTURES REQUEST
            //
            var cLFixturesClient = new RestClient("https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx&competition_id=244");
            var cLFixturesRequest = new RestRequest(Method.GET);
            IRestResponse cLFixturesResponse = cLFixturesClient.Execute(cLFixturesRequest);

            var cLFixturesResponseDeserialized = JsonConvert.DeserializeObject<FixturesRootobject>(cLFixturesResponse.Content);

            arrayIndex = 0;
            foreach (Fixture a in cLFixturesResponseDeserialized.data.fixtures)
            {
                championsLeagueFixtures[arrayIndex] = new string[] { a.home_name, " v ", a.away_name, a.date };
                arrayIndex++;
            }

        }


        //BUTTONS
        //The buttons add the respective league table string arrays to the league table listview

        private void premierLeagueButton_Click(object sender, EventArgs e)
        {
            LeagueButtonClick(); //This is a method for cleaning house each time we select a new league
            premierLeagueButton.BackColor = Color.Red;
            competitionID = "2";
            scoresButton.Visible = true;
            fixturesButton.Visible = true;

            //We loop through the 2D league array and add each 1D array within it
            //to the listview as a new listview item
            for (int i = 0; i < premierLeagueTable.GetLength(0); i++)
            {
                leagueTableListview.Items.Add(new ListViewItem(premierLeagueTable[i]));
            }
        }

        private void bundesligaButton_Click(object sender, EventArgs e)
        {
            LeagueButtonClick();
            bundesligaButton.BackColor = Color.Red;
            competitionID = "1";

            //We loop through the 2D league array and add each 1D array within it
            //to the listview as a new listview item
            for (int i = 0; i < bundesligaTable.GetLength(0); i++)
            {
                leagueTableListview.Items.Add(new ListViewItem(bundesligaTable[i]));
            }
        }

        private void laLigaButton_Click(object sender, EventArgs e)
        {
            LeagueButtonClick();
            laLigaButton.BackColor = Color.Red;
            competitionID = "3";

            //We loop through the 2D league array and add each 1D array within it
            //to the listview as a new listview item
            for (int i = 0; i < laLigaTable.GetLength(0); i++)
            {
                leagueTableListview.Items.Add(new ListViewItem(laLigaTable[i]));
            }
        }

        private void serieAButton_Click(object sender, EventArgs e)
        {
            LeagueButtonClick();
            serieAButton.BackColor = Color.Red;
            competitionID = "4";

            //We loop through the 2D league array and add each 1D array within it
            //to the listview as a new listview item
            for (int i = 0; i < serieATable.GetLength(0); i++)
            {
                leagueTableListview.Items.Add(new ListViewItem(serieATable[i]));
            }
        }


        private void ligue1Button_Click(object sender, EventArgs e)
        {
            LeagueButtonClick();
            ligue1Button.BackColor = Color.Red;
            competitionID = "5";

            //We loop through the 2D league array and add each 1D array within it
            //to the listview as a new listview item
            for (int i = 0; i < ligue1Table.GetLength(0); i++)
            {
                leagueTableListview.Items.Add(new ListViewItem(ligue1Table[i]));
            }
        }


        private void championsLeagueButton_Click(object sender, EventArgs e)
        {
            LeagueButtonClick();
            leagueTableListview.Visible = false;
            championsLeagueButton.BackColor = Color.Red;
            competitionID = "244";

            //topScorerButton.Visible = false; //the CL top scorer isn't working right now because Haaland has no photo in the SoccersAPI database

            //We populate each group listview with its respective array
            //which we created earlier when the form loaded
            listViewGroupA.Visible = true;
            groupALabel.Visible = true;

            //We loop through the 2D group arrays and add each 1D array within them
            //to their own group's listview as a new listview item
            for (int i = 0; i < 4; i++)
            {
                listViewGroupA.Items.Add(new ListViewItem(championsLeagueGroupATable[i]));
                listViewGroupB.Items.Add(new ListViewItem(championsLeagueGroupBTable[i]));
                listViewGroupC.Items.Add(new ListViewItem(championsLeagueGroupCTable[i]));
                listViewGroupD.Items.Add(new ListViewItem(championsLeagueGroupDTable[i]));
                listViewGroupE.Items.Add(new ListViewItem(championsLeagueGroupETable[i]));
                listViewGroupF.Items.Add(new ListViewItem(championsLeagueGroupFTable[i]));
                listViewGroupG.Items.Add(new ListViewItem(championsLeagueGroupGTable[i]));
                listViewGroupH.Items.Add(new ListViewItem(championsLeagueGroupHTable[i]));
            }

            //And now we are ready to display the 8 group tables 
            listViewGroupB.Visible = true;
            groupBLabel.Visible = true;

            listViewGroupC.Visible = true;
            groupCLabel.Visible = true;

            listViewGroupD.Visible = true;
            groupDLabel.Visible = true;

            listViewGroupE.Visible = true;
            groupELabel.Visible = true;

            listViewGroupF.Visible = true;
            groupFLabel.Visible = true;

            listViewGroupG.Visible = true;
            groupGLabel.Visible = true;

            listViewGroupH.Visible = true;
            groupHLabel.Visible = true;
        }

        private void HideAllControls()
        {
            listViewGroupA.Visible = false;
            groupALabel.Visible = false;

            listViewGroupB.Visible = false;
            groupBLabel.Visible = false;

            listViewGroupC.Visible = false;
            groupCLabel.Visible = false;

            listViewGroupD.Visible = false;
            groupDLabel.Visible = false;

            listViewGroupE.Visible = false;
            groupELabel.Visible = false;

            listViewGroupF.Visible = false;
            groupFLabel.Visible = false;

            listViewGroupG.Visible = false;
            groupGLabel.Visible = false;

            listViewGroupH.Visible = false;
            groupHLabel.Visible = false;
        }

        private void topScorerButton_Click(object sender, EventArgs e)
        {
            //This Tooltip is supposed to offer a tip when you hover over the golden boot button:
            //"Get top scorers". But for some reason it rarely works
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.topScorerButton, "Get League Top Scorers");
            HideAllControls();

            topScorerListView.Items.Clear();
            fixturesListView.Items.Clear();
            fixturesListView.Visible = false;
            topScorerPictureBox.Image = null;
            fixturesLabel.Visible = false;
            resultsLabel.Visible = false;            
            leagueTableListview.Visible = false;            
            topScorerListView.Visible = true;

            //We make a request based on the string competitionID,
            //which is set each time we select a league through its button in the app
            var client = new RestClient("https://livescore-api.com/api-client/competitions/goalscorers.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx");
            var request = new RestRequest(Method.GET);
            request.AddParameter("competition_id", competitionID);
            //For reference -> competition ids are: Bundesliga - 1, Premier League - 2; La Liga - 3; Serie A - 4; Ligue 1 - 5; Turkey - 6; Champions League - 244;
            IRestResponse response = client.Execute(request);

            var responseDeserialized = JsonConvert.DeserializeObject<TopScorersRootobject>(response.Content);

            //We add the name and goal tally of the top scorer to some labels which
            //We will display under their photograph later
            //We store their name in the string variable storeTopScorer because we may edit some of the top scorer name strings later to make them work in a second API request
            topScorer = responseDeserialized.data.goalscorers.First().name;
            storeTopScorer = topScorer;
            topScorerNameLabel.Text = topScorer;
            topScorerGoalsLabel.Text = responseDeserialized.data.goalscorers.First().goals + " goals";

            //We have to manipulate some of the top scorer names here in order to successfully find them in 
            //the Soccersapi query request: the search returns incorrect players or null otherwise.
            //I know this is not pretty but it works
            if (topScorer.Contains("Suarez"))
            {
                topScorer = "Luis Alberto Suarez Diaz";
            }
            else if (topScorer.Contains("Lukaku"))
            {
                topScorer = "Romelu Menama Lukaku Bolingoli";
            }
            else if (topScorer.Contains("Mbappe"))
            {
                topScorer = "Mbappe";
            }
            else if (topScorer.Contains("Messi"))
            {
                topScorer = "Lionel Andres Messi";
            }
            else if (topScorer.Contains("Haaland"))
            {
                topScorer = "Erling Braut Haaland";
            }
            //    

            //Next we add the top 20 goalscorers to a rankings list
            foreach (Goalscorer a in responseDeserialized.data.goalscorers)
            {
                string[] rankedTopGoalscorer = new string[] { a.name, a.goals, a.team.name };
                topScorerListView.Items.Add(new ListViewItem(rankedTopGoalscorer));
            }


            //QUERY REQUEST TESTED IN SoccersAPITest APP
            //------------------

            //Here we send a search request to our SoccersAPI client
            //where a paramater of the search is the name of our top scorer as a query
            //The query parameter is built into the client url here
            var queryclient = new RestClient("https://api.soccersapi.com/v2.2/search/?user=" + "ENTER_SOCCERSAPI_USER_HERE" + "&token=" + "ENTER_SOCCERSAPI_TOKEN_HERE"  + topScorer);
            var queryrequest = new RestRequest(Method.GET);
            //request.AddParameter("q", topScorer);
            IRestResponse queryresponse = queryclient.Execute(queryrequest);
            // For reference -- > Teams: Liverpool ID = 1, Chelsea ID = 20; Players: Salah ID = 6390; Leagues: Premier League -583

            var queryresponseDes = JsonConvert.DeserializeObject<QueryRootobject>(queryresponse.Content);

            //We deserialize the response and get the ID paramater of the top player result in our search
            //Then we use that id to create a url to that player's photo in the SoccersAPI database
            //It's a little bit of a hack, I know, but we can't get them to respond with images to our request on our free subscription
            topScorerID = queryresponseDes.data.First().id;
            topScorerImageUrl += topScorerID + ".png";
            topScorerPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            if (topScorer.Contains("Haaland"))
            {
                topScorerPictureBox.Image = Image.FromFile(@"Resources\haaland.jpg");
            }
            else
            {
                topScorerPictureBox.Load(topScorerImageUrl);
            }            
            topScorerPictureBox.Visible = true;
            topScorerNameLabel.Visible = true;
            topScorerGoalsLabel.Visible = true;

            //We clean the topScorer variables here in case the user double clicks the getTopScorer button
            topScorer = storeTopScorer;
            topScorerID = "";
            topScorerImageUrl = "https://cdn.soccersapi.com/images/soccer/players/100/";
        }


        private void scoresButton_Click(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.topScorerButton, "Get Match Results");

            LeagueButtonClick();

            //We make the relevant controls visible to display the results
            fixturesLabel.Visible = false;
            resultsLabel.Visible = true;
            leagueTableListview.Visible = false;
            fixturesListView.Visible = true;

            switch (competitionID)
            {
                case "1":
                    resultsLabel.Text = "Bundesliga Results";
                    bundesligaButton.BackColor = Color.Red;
                    for (int i = 0; i < bundesligaResults.Length; i++)
                    {
                        if (bundesligaResults[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(bundesligaResults[i]));
                        }
                    }
                    break;
                case "2":
                    resultsLabel.Text = "Premier League Results";
                    premierLeagueButton.BackColor = Color.Red;
                    for (int i = 0; i < premierLeagueResults.Length; i++)
                    {
                        if (premierLeagueResults[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(premierLeagueResults[i]));
                        }                        
                    }
                    break;
                case "3":
                    resultsLabel.Text = "La Liga Results";
                    laLigaButton.BackColor = Color.Red;
                    for (int i = 0; i < laLigaResults.Length; i++)
                    {
                        if (laLigaResults[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(laLigaResults[i]));
                        }                        
                    }
                    break;
                case "4":
                    resultsLabel.Text = "Serie A Results";
                    serieAButton.BackColor = Color.Red;
                    for (int i = 0; i < serieAResults.Length; i++)
                    {
                        if (serieAResults[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(serieAResults[i]));
                        }
                    }
                    break;
                case "5":
                    resultsLabel.Text = "Ligue 1 Results";
                    ligue1Button.BackColor = Color.Red;
                    for (int i = 0; i < ligue1Results.Length; i++)
                    {
                        if (ligue1Results[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(ligue1Results[i]));
                        }
                    }
                    break;
                case "244":
                    resultsLabel.Text = "Champions League Results";
                    championsLeagueButton.BackColor = Color.Red;
                    for (int i = 0; i < championsLeagueResults.Length; i++)
                    {
                        if (championsLeagueResults[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(championsLeagueResults[i]));
                        }
                    }
                    break;
            }
        }

        private void fixturesButton_Click(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.topScorerButton, "Get Match Fixtures");

            //We make the relevant controls visible to display the fixtures
            LeagueButtonClick();
            fixturesLabel.Visible = true;
            resultsLabel.Visible = false;
            leagueTableListview.Visible = false;
            fixturesListView.Visible = true;

            switch (competitionID)
            {
                case "1":
                    fixturesLabel.Text = "Bundesliga Fixtures";
                    bundesligaButton.BackColor = Color.Red;
                    for (int i = 0; i < bundesligaFixtures.Length; i++)
                    {
                        if (bundesligaFixtures[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(bundesligaFixtures[i]));
                        }                        
                    }
                    break;
                case "2":
                    fixturesLabel.Text = "Premier League Fixtures";
                    premierLeagueButton.BackColor = Color.Red;
                    for (int i = 0; i < premierLeagueFixtures.Length; i++)
                    {
                        if (premierLeagueFixtures[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(premierLeagueFixtures[i]));
                        }
                    }
                    break;
                case "3":
                    fixturesLabel.Text = "La Liga Fixtures";
                    laLigaButton.BackColor = Color.Red;
                    for (int i = 0; i < laLigaFixtures.Length; i++)
                    {
                        if (laLigaFixtures[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(laLigaFixtures[i]));
                        }
                    }
                    break;
                case "4":
                    fixturesLabel.Text = "Serie A Fixtures";
                    serieAButton.BackColor = Color.Red;
                    for (int i = 0; i < serieAFixtures.Length; i++)
                    {
                        if (serieAFixtures[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(serieAFixtures[i]));
                        }
                    }
                    break;
                case "5":
                    fixturesLabel.Text = "Ligue 1 Fixtures";
                    ligue1Button.BackColor = Color.Red;
                    for (int i = 0; i < ligue1Fixtures.Length; i++)
                    {
                        if (ligue1Fixtures[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(ligue1Fixtures[i]));
                        }
                    }
                    break;
                case "244":
                    fixturesLabel.Text = "Champions League Fixtures";
                    championsLeagueButton.BackColor = Color.Red;
                    for (int i = 0; i < championsLeagueFixtures.Length; i++)
                    {
                        if (championsLeagueFixtures[i] != null)
                        {
                            fixturesListView.Items.Add(new ListViewItem(championsLeagueFixtures[i]));
                        }
                    }
                    break;
            }
        }


        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
        }

        private void minimiseButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Allowing us to move the form with click and drag
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }

        public void LeagueButtonClick()
        {
            //This is the action to be taken each time a new league is selected
            //We are essentially cleaning house in this method
            premierLeagueButton.BackColor = default;
            bundesligaButton.BackColor = default;
            laLigaButton.BackColor = default;
            serieAButton.BackColor = default;
            ligue1Button.BackColor = default;
            championsLeagueButton.BackColor = default;

            topScorerImageUrl = "https://cdn.soccersapi.com/images/soccer/players/100/";

            welcomePictureBox.Visible = false;
            welcomePictureBox2.Visible = false;
            leagueTableListview.Items.Clear();
            fixturesListView.Items.Clear();
            topScorerPictureBox.Image = null;            
            topScorerListView.Visible = false;
            topScorerNameLabel.Text = "";
            topScorerGoalsLabel.Text = "";
            fixturesListView.Visible = false;
            fixturesLabel.Visible = false;
            resultsLabel.Visible = false;

            groupALabel.Visible = false;
            listViewGroupA.Visible = false;
            listViewGroupA.Items.Clear();

            groupBLabel.Visible = false;
            listViewGroupB.Visible = false;
            listViewGroupB.Items.Clear();

            groupCLabel.Visible = false;
            listViewGroupC.Visible = false;
            listViewGroupC.Items.Clear();

            groupDLabel.Visible = false;
            listViewGroupD.Visible = false;
            listViewGroupD.Items.Clear();

            groupELabel.Visible = false;
            listViewGroupE.Visible = false;
            listViewGroupE.Items.Clear();

            groupFLabel.Visible = false;
            listViewGroupF.Visible = false;
            listViewGroupF.Items.Clear();

            groupGLabel.Visible = false;
            listViewGroupG.Visible = false;
            listViewGroupG.Items.Clear();

            groupHLabel.Visible = false;
            listViewGroupH.Visible = false;
            listViewGroupH.Items.Clear();

            leagueTableListview.Visible = true;
            topScorerButton.Visible = true;
            scoresButton.Visible = true;
            fixturesButton.Visible = true;
        }


        //LEAGUE TABLES CLASSES (REST RESPONSE JSON CONVERTED TO C# CLASSES)
        //Heisenbug API --> (https://heisenbug-premier-league-live-scores-v1.p.rapidapi.com/api/premierleague/table)
        public class LeagueTableRootObject
        {
            public Record[] records { get; set; }
        }

        public class Record
        {
            public string team { get; set; }
            public int played { get; set; }
            public int win { get; set; }
            public int draw { get; set; }
            public int loss { get; set; }
            public int goalsFor { get; set; }
            public int goalsAgainst { get; set; }
            public int points { get; set; }
        }
        //

        //CHAMPIONS LEAGUE GROUP TABLE CLASSES
        //Heisenbug API --> (https://heisenbug-champions-league-live-scores-v1.p.rapidapi.com/api/championsleague/table?group=H)
        public class ChampionsLeagueGroupRootobject
        {
            public CLGroupRecord[] records { get; set; }
        }

        public class CLGroupRecord
        {
            public string team { get; set; }
            public int played { get; set; }
            public int win { get; set; }
            public int draw { get; set; }
            public int loss { get; set; }
            public int goalsFor { get; set; }
            public int goalsAgainst { get; set; }
            public int points { get; set; }
        }
        //


        //TOP SCORERS CLASSES
        //Live-Score-API --> (https://livescore-api.com/api-client/competitions/goalscorers.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrxS)
        public class TopScorersRootobject
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Competition competition { get; set; }
            public Goalscorer[] goalscorers { get; set; }
        }

        public class Competition
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Goalscorer
        {
            public string player_id { get; set; }
            public string name { get; set; }
            public string team_id { get; set; }
            public string goals { get; set; }
            public string assists { get; set; }
            public string played { get; set; }
            public string competition_id { get; set; }
            public string season_id { get; set; }
            public string edition_id { get; set; }
            public Team team { get; set; }
        }

        public class Team
        {
            public string id { get; set; }
            public string name { get; set; }
            public string stadium { get; set; }
            public string location { get; set; }
        }
        //

        //QUERY CLASSES
        //SoccersAPI --> (https://api.soccersapi.com/v2.2/search/?user=shanster8592&token=9b373f21ff69b7843c964dbe93524c89&t=all&q=Cristiano%20Ronaldo)
        //We get the name of the top scorer from Live-Score-API
        //We use the name of that top scorer as a query paramater in a SoccersAPI request
        //We then get that players ID in the SoccersAPI database
        //We finally use that ID to get a url link to an image of that player
        public class QueryRootobject
        {
            public Datum[] data { get; set; }
            public Meta meta { get; set; }
        }

        public class Meta
        {
            public int requests_left { get; set; }
            public string user { get; set; }
            public string plan { get; set; }
            public int pages { get; set; }
            public object page { get; set; }
            public int total { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string name { get; set; }
            public string country_name { get; set; }
            public string cc { get; set; }
            public string type { get; set; }
        }


        //SCORE RESULTS CLASSES
        //Live-Score-API -->http://livescore-api.com/api-client/scores/history.json?key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx
        public class ResultsRootobject
        {
            public bool success { get; set; }
            public ResultsData data { get; set; }
        }

        public class ResultsData
        {
            public string next_page { get; set; }
            public bool prev_page { get; set; }
            public int total_pages { get; set; }
            public ResultsMatch[] match { get; set; }
        }

        public class ResultsMatch
        {
            public int home_id { get; set; }
            public string added { get; set; }
            public object location { get; set; }
            public Outcomes outcomes { get; set; }
            public string away_name { get; set; }
            public string competition_name { get; set; }
            public object scheduled { get; set; }
            public string ft_score { get; set; }
            public string score { get; set; }
            public string home_name { get; set; }
            public int competition_id { get; set; }
            public int league_id { get; set; }
            public int id { get; set; }
            public string h2h { get; set; }
            public int away_id { get; set; }
            public string time { get; set; }
            public string events { get; set; }
            public string status { get; set; }
            public string et_score { get; set; }
            public string last_changed { get; set; }
            public string date { get; set; }
            public int fixture_id { get; set; }
            public string ht_score { get; set; }
        }

        public class Outcomes
        {
            public string extra_time { get; set; }
            public string half_time { get; set; }
            public string full_time { get; set; }
        }
        //

        //FIXTURES CLASSES
        //Live-Score-API -->https://livescore-api.com/api-client/fixtures/matches.json?&key=D5nLqw26cZiXS7sH&secret=g8b7hVxdOzuOqfi4kFPKcSRoR9mJgOrx&competition_id=244
        public class FixturesRootobject
        {
            public bool success { get; set; }
            public FixturesData data { get; set; }
        }

        public class FixturesData
        {
            public Fixture[] fixtures { get; set; }
            public string next_page { get; set; }
            public bool prev_page { get; set; }
        }

        public class Fixture
        {
            public string id { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public string round { get; set; }
            public string home_name { get; set; }
            public string away_name { get; set; }
            public string location { get; set; }
            public string league_id { get; set; }
            public string competition_id { get; set; }
            public string home_id { get; set; }
            public string away_id { get; set; }
            public FixturesCompetition competition { get; set; }
            public League league { get; set; }
        }

        public class FixturesCompetition
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class League
        {
            public string id { get; set; }
            public string name { get; set; }
            public string country_id { get; set; }
        }
        //

    }
}
