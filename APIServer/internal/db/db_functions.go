package db

import (
	"APIServer/internal/models"
	"database/sql"
	"errors"
	"fmt"

	_ "github.com/lib/pq"
)

const (
	host     = "localhost"
	port     = 5432
	user     = "postgres"
	password = "314159"
	dbname   = "warehousedb"
)

func OpenDB() (*sql.DB, error) {
	psqlconn := fmt.Sprintf("host=%s port=%d user=%s password=%s dbname=%s sslmode=disable", host, port, user, password, dbname)
	db, err := sql.Open("postgres", psqlconn)
	if err != nil {
		return nil, err
	}
	return db, nil
}

func GetUser(db *sql.DB, username string, password string) (models.User, error) {

	rows, err := db.Query(`SELECT id, name, surname, role
	FROM public."Users" WHERE username = $1 AND password = $2`, username, password)

	if err == sql.ErrNoRows {
		return models.User{}, err
	} else if err != nil {
		return models.User{}, err
	}

	defer rows.Close()

	var user models.User
	found := false
	for rows.Next() {
		if err := rows.Scan(&user.ID, &user.Name, &user.Surname, &user.Role); err != nil {
			return models.User{}, err
		}
		found = true
	}

	if !found {
		return models.User{}, errors.New("No rows")
	}

	return user, nil
}
