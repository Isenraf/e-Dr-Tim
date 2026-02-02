using System;
using System.IO;
using BANA.Models;
using RestSharp;




namespace BANA.MyFunction
{
    public class SendOnline
    {
        //private readonly ResultatContext _context;
        // public SendOnline(ResultatContext context)
        // { 
        //      _context=context;

        // }

        public SendOnline(Resultat monresultat)
        { 
            SendResultOnline(monresultat);
        }
         public static async Task SendResultOnline(Resultat resultat)
        {

            var url = "https://www.polycliniquedepoitiers.com/Resultat/AddTrump";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);    
            resultat.Id=0;
            request.AddJsonBody(resultat);
                
            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            //Console.WriteLine(output);

        }


        // http://localhost:5180/

        // public  void ft(int ligne)
        // {
           

            
        // }

        

        

    }
}
