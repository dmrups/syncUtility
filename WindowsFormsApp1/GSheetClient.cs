using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
    class GSheetClient
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        string[] Scopes = { SheetsService.Scope.Spreadsheets };
        string ApplicationName = "Google Sheets API .NET Quickstart";
        string spreadsheetId = "17W-oH5sZboEG6Bzmp9R0F55DRWXPaqixU93vsU6ldW8";
        UserCredential credential;

        public GSheetClient()
        {
            using (var stream =
                            new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
        }

        public void InsertRows(IList<IList<object>> data, string sheetName)
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            String range = sheetName;
            var vr = new ValueRange
            {
                Range = range,
                Values = data
            };

            SpreadsheetsResource.ValuesResource.AppendRequest request2 =
                service.Spreadsheets.Values.Append(vr, spreadsheetId, range);

            request2.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            request2.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            AppendValuesResponse response = request2.Execute();
        }
    }
}
