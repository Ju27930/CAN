# CAN_client

+ Client TCP pour serveur nodejs
Petit client de discution avec connection tcp et login via mysql

+ TODO
Afficher la liste des clients connectés
...



# Le serveur en lui meme :
```
var net = require("net");
var mysql = require('mysql');
var server = net.createServer();
var connection = mysql.createConnection({
    host     : 'localhost',
    user     : 'USERDELABASE',
    password : 'PASSWDDELABASE',
    database : 'DATABASE',
    wait_timeout : '600000'
});


var testco = false;
var socketCo = [];

server.on("connection", function (socket) {
    var remoteAddress = socket.remoteAddress + ":" + socket.remotePort;
    console.log("nouveau client connecté %s", remoteAddress);
	socket.name = remoteAddress;
	socketCo.push(socket);
	
    socket.on("data", function (d) {
        var s = String(d);
        var s2 = s.split("|");
        var instruction = s2[0];
        
        if (instruction == "LOGIN") {
            var mail = s2[1];
            var passwd = s2[2];
            socket.pause();
            console.log("connection avec mail, pseudo...: " + mail + ", " + passwd)
            
            mail = mail.replace("\@", "\\@");
            var queryLogin = 'SELECT * from login WHERE mail="' + mail + '" AND passwd="' + passwd + '"';
            connection.query(queryLogin, function (err, rows, fields) {
                if (!err) {
                    console.log('The solution is: ', rows[0]);
                    if (rows[0] == null) {
                        socket.write("ERREURLOGIN");
                    }
                    else {
			socket.name = mail;
                        console.log("user conneté");
                        socket.resume();
                        socket.write(socket.name+ " |nous a rejoint\n");
						//.write(socketCo);
						broadcast(socket.name + " |nous a rejoint\n", socket); 
						
                    }
                }
                else {
                    console.log('Error while performing Query.');
                    socket.write("erreur query"); 
            
                }
              
                   

                        
        
            });
    }
	
			
	
     else {
		//socket.write("recu");
		broadcast(socket.name + " |" + s + "\n", socket);       
        }

        console.log("Data from %s: %s", remoteAddress, d);
    });

    socket.once("close", function () {
        console.log("Client deconnecté %s", remoteAddress);
		socketCo.splice(socketCo.indexOf(socket), 1); 
		broadcast(socket.name + " |nous a quitté.\n", socket); 

    });

    socket.on("error", function () { });
});


// Send a message to all  
   function broadcast(message, sender) { 
     socketCo.forEach(function (sockCo) { 
       // Don't want to send it to sender 
       //if (sockCo === sender) return; 
       sockCo.write(message); 
     }); 
     // Log it to the server output too 
     process.stdout.write(message) 
   } 


server.listen(9000, function () {
    console.log("Le serveur ecoute sur %j", server.address());
    
});
```




