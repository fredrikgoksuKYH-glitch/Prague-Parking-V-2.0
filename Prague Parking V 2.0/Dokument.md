DAG 1

Planerar struktur efter kriterier

Strukturen vi är ute efter är att separera UI och Kärnlogik till en början. Vi kommer med störst sanorligenhet att 
ändra och justera under projektet.
En bil av det hela redan vid start (reserverar rättigheterna för ändringar :100:

UI via Specter.Console -> GarageOvervie,MenuView och Program.cs
Kärnlogik : Models(Vehicle och dess subklasser) ParkingGarage.
Program.cs ska vara där allt möts och blir tillkallat till.

Mapar Huvudmap,Config,Models,Data,Interface

Huvudmap :  Här ligger alla maps ink program.cs
			Program.cs  ( Maps )

	UI/View:
			GarageView		(Ritar ut garaget)
			MenuView		(Hanterar input och dialoger)
	Config:
			GarageConfig	(Inställningar för p-ruttor,våningar och taxor)
	✓ Models✓
	✓       Vehicles		(Abstract klass "Mall" med regnr,tid,storlek)
	✓		Car			(Subklass)
	✓		MC			(Subklass)
	✓		Bus			(Subklass)
	✓		Bike			(Subklass)
			ParkingSpot		(Parkings ruttans egenskaper (Kapacitet)
			ParkingGarage		(Kärnmotor för garaget (parkera,flytta,söka och räkna ut pris))
	Data
			FileRepos	(via Json)
			ConfigRepos	(via Json)
	Interface
			IVehicle	
			iParkingSpot


# Tankar under projektet.
Aldrig rört json files innan så där blir det lite googling för att navigera sig fram.

# Dag 1
1. Mindmap/Struktur karta
2. Json file uppsätning 
   Skappar PragueParking.Core och kopplar ihop den med PPv2.0
   (PP V2.0 ->Dependencies->Add Project Reference->PragueParking.Core)
3. config.json verkade inte komma med så fick sätta till den för hand.
   Planering inför projektet ( med json.file ), eftersom json file är nytt för en själv så
   behöver jag ta reda på vad json.file är och vad skillnaden är med och utan json.file.

# Dag 2
Skappat Model mapen med 
Vehicle.cs
Bike.cs
MC.cs
Car.cs
Bus.cs
Gjort Vehicle till en abstract klass som ska funka som en mall
för alla fordon med gemmensamma variablar.
ParkingSpot var en aning tuffare då Json.Files inte är en vana sen
innan men fick läsa på om JsonIgnore och JsonConstructor

# JsonIgnore
kan användas om vill ignorera något t.ex 
ParkingsTid och Aktuell kostnad. Eftersom den kommer att spara
EXAKT det som var när programmet stängdes så kommer kostnaden och tiden
blir väldigt konstigt mellan stängning och öppning av programmet.
# JsonConstructor
Behovet av JsonConstrctor och arv för fodon.

Vi har två olika väger in i objektet:100:
1. Ena används när fordonet körs in i garagets och sätter
CheckInTime till DateTime.Now
2. Serialiseringskonstruktor som endast används när
System.Text.Json läser in data från filen.
Den tar emot både registreringsnummer och CheckInTime.
