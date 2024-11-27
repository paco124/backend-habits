using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace PartsServer
{
    public class FirestoreHelper
    {
        static string fireconfig = @"
            {
              ""type"": ""service_account"",
              ""project_id"": ""backend-habitos"",
              ""private_key_id"": ""e33239a95ba6a4753618c1b5ac045ca19dca0590"",
              ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCygQQC8wwZD6j0\nBoRUBDJNK9fqXt4YH6qru1DGOXIBFnP/Kj4ZM6szQTp9QIerNkqcdlp5u0zu7Syn\nLQYM7jOezbaL4QJsZCAo7wcQsixZAwifvvLaDIDuysYtR3QKpG4YKO6ELO7bqCgD\nfDPwEb7gd6kNbCqmWwb4jeynOgI7vFZY7Dcwvj+NlKBDh8ADMAawvRDSkiWoQML7\n1yfwYJcJKBRPhQb2ZM/x65hhoxSsbORkGyxk9BK95RAtAALQGgslHn6ssPTuao+i\n3y1bDJfpt+qMea0lqb8Qh4B9nnnEofOXjMxjupO8f7SddEP3+oaJNlN+8NfqQ27U\n4fpdd5GHAgMBAAECggEACG1a2SDMU2qP2WVtXeIM1ePZ/7CW36CvGAhx2cngffin\nZUaDRAFSdcsAjeU8AnM/tn9H6jNTgHwl/YZ8oKOmQDsKATSQPrnDYutV9A3VViZ0\ng8YqBO1OMPfVv1F+QvpcUDuudwMAC9K2gOOeOBk+X66GhqGyqcPmYlmg8IwmitdX\nL+ytyUBfmRrhfM/1XkWAxWIw+ZTMVTCpomz/BCyehOQ7fd3hO20vye1mGga8LBCL\ny+gBh9LJ+Lr711i99fnLywrf89V1aVl0lJpFxDWxXjeKu0XmSxjMEbJCO/Iy1OTO\niH/mTrk8DUkGaybMGZAMDRUbqeWnR5cM3M+Za7mfMQKBgQDfg6aUvAqjBAW8hjuF\nCiIpC/EFatt11NoDlvAD5+1Qs2bSJ45jJcmSA6gdeMhspffgBTK3IyGv/rGCIi/Q\nk82d/UQlv6FaMDEIitNogDVmFPvKvtjBbkwaRiTvXzdXJqk7QL3XKStQYll2T0Dm\nvwcZtPVKCesw+8QcErsCtPnA6wKBgQDMcqf0YADSMk9Lr/CJ43gvJx3toyPpahKd\nYWAnaUmEGZ937e5wvQuA28wAARxWC7Ft6Rn7zskivsqXnOKfOn+UTjB7H0AnK4lL\nrmXtBudIA1NLwP/xbPluTmmF2Vx65NPdduBERwI1aJ9mgLzGmr0DfUfCjD76EOPm\nDGpMmyqq1QKBgCx61IGw0iUSvdmAlQOqupWUjMhZNYdDbodcWDNOyklPCl00Bf/x\nGyX1mFUVHj2Q4b0xC6CqWx/c/ZI6H23QnBCewBsLAZ8jC/75MY0QRpAkCN+WDyif\nPIHWB6+jMS4kwXTLM3xH8xiyGb+TkgTZax5QhqfPRYCf+azkhVbKkRE1AoGAHYh2\ngJOZqsN/tuQ8b/6+7rjM1vYZCu+6rqdRV4AbEY0N/yoMppZ4Ye3eQyXl7PwVblYc\n3cYaQf2jkFEmX2/42/iLgk2aW84rMBaKPLohkMKCNtAzaLGgOnHzRlWGyW3iuPwi\nG1rSk9qAJDN2kY3qveVMB2tw8XidIy8p79T+Td0CgYAvCZBIcpXDYu77CsS04TvQ\nlUJWPgQnm7I40nu82zBKqsiHDYzNj7mxtRdsFNKHVmFNFZ0KhCrhbwiAravA3J3b\n4znEiWoRYXfLGZgW3jyN/PziPXdfo9i/YsKJX6rdIoI277jhAykzVD5QL/XR9DdQ\nFAWmgHz57ZrhLzVTUrQc6w==\n-----END PRIVATE KEY-----\n"",
              ""client_email"": ""firebase-adminsdk-iq76u@backend-habitos.iam.gserviceaccount.com"",
              ""client_id"": ""105472877124412681343"",
              ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
              ""token_uri"": ""https://oauth2.googleapis.com/token"",
              ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
              ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-iq76u%40backend-habitos.iam.gserviceaccount.com"",
              ""universe_domain"": ""googleapis.com""
            }        
        ";
        static string filepath = "";
        public static FirestoreDb Database { get; private set; }
        public static void SetEnviromentVariable()
        {
            filepath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())) + ".json";
            File.WriteAllText(filepath, fireconfig);
            File.SetAttributes(filepath, FileAttributes.Hidden);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            Database = FirestoreDb.Create("backend-habitos");
            File.Delete(filepath);
        }
    }
}
