DAG 1

Planerar struktur efter kriterier

PPV2.0
	|
	PP.Core:
	|
	|Config
	|	|
	|	|_GarageConfig
	|
	|Models
	|	|
	|	|_Vehicles
	|	|_Car
	|	|_MC
	|	|_Bus
	|	|_ParkingSpot
	|	|_ParkingGarage
	|
	|Data
	|	|
	|	|_FileRepository
	|	|_ConfigRespository
	|
	|Interface
	|	|
	|	|_InterfaceVehicles
	|	|_InterfaceParkingSpots

Skappar PragueParking.Core och kopplar ihop den med PPv2.0

PP V2.0 ->Dependencies->Add Project Reference->PragueParking.Core


