package main

import (
	server "APIServer/internal"
	"APIServer/internal/handlers"
	"APIServer/internal/repository"
	"log"
	"os"

	"github.com/joho/godotenv"
)

func main() {
	if err := godotenv.Load(); err != nil {
		log.Fatalf("%s", err.Error())
	}

	cfg := repository.LoadConfig()

	if len(os.Args) == 2 {
		command := os.Args[1]
		switch command {
		case "migration":
			if err := repository.MakeMigration(cfg); err != nil {
				log.Fatalf("%s", err.Error())
				return
			}
			log.Println("Migration completed!")
			os.Exit(0)
		default:
			panic("bad argument")
		}
	}

	db, err := repository.OpenDB(cfg)
	if err != nil {
		log.Fatal(err)
	}

	repository := repository.NewRepository(db)
	handlers := handlers.NewHandlers(repository)
	server := server.NewServer()
	if err := server.Run(os.Getenv("SERVER_PORT"), handlers.InitRoutes()); err != nil {
		return
	}
}

// db.AddItemsByConsole(dbase)
