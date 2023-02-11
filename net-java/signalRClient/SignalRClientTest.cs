using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.signalRClient
{
    public class SignalRClientTest
    {
        public async Task Test()
        {
            //using (var hubConnection = new HubConnection("http://localhost:5204", "access_token=", false))
            //{
            //    IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("AutoStockstoreEquHub");
            //    //stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            //    //ServicePointManager.DefaultConnectionLimit = 10;
            //    await hubConnection.Start();

            //    await stockTickerHubProxy.Invoke("ExecuteTask", "1");
            //}

            var connection = new HubConnectionBuilder()
.WithUrl("http://localhost:5204/AutoStockstoreEquHub", options =>
{
    options.AccessTokenProvider = async () =>
    {
        // Get and return the access token.

        return await Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxIiwiRW1wTm8iOiJhZG1pbiIsIkVtcE5tIjoiYWRtaW4iLCJQcm92aWRlciI6IjAiLCJpYXQiOiIyMDIyLzExLzI5IDE6MTI6NTgiLCJleHAiOjE2NzQ4NjgzNzgsImlzcyI6ImhkZ20iLCJhdWQiOiJXZWIifQ.vfbL1WKV6YWgwzM-hyHXxISyxuDaycSLTxhY58y0WrI");
    };
})
.Build();

            await connection.StartAsync();

            await connection.SendAsync("ExecuteTask", 1);
        }
    }
}