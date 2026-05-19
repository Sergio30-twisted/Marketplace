using APIGateWay_MarketPlace.Services;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.Intrinsics.X86;

namespace APIGateWay_MarketPlace.Hubs
{
    public class NotificacionesHub : Hub
    {

        //Hub es la clase de SignalR que representa el
        //canal WebSocket. Es como la "puerta" que el cliente toca para conectarse.


        //Este método se ejecuta automáticamente cada vez que un cliente se conecta. Es como un evento "bienvenida".


        //Cuando el cliente abre la app, el NotificacionesHub
        //y crea los grupos para cada usuario. El cliente se conecta a su grupo usando su userId.
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, userId!);
            await base.OnConnectedAsync();
        }

        //    El cliente abre la app
        //→ SignalR construye el puente(conexión WebSocket)
        //→ OnConnectedAsync mete ese puente al cuarto "42"


        //    Llega un evento de RabbitMQ
        //→ EventBusConsumer lo agarra
        //→ Le dice al HubContext: "manda esto al cuarto 42"
        //→ HubContext entra al cuarto y grita el mensaje
        //→ Todos los puentes en ese cuarto lo reciben

    }
}
