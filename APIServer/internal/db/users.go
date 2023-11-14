package db

import (
	"APIServer/internal/models"
	"database/sql"
	"errors"
	"fmt"
)

func GetAllUsers(db *sql.DB) ([]models.User, error) {
	rows, err := db.Query(`SELECT id, name, surname, role
	FROM public."Users"`)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	var users []models.User
	for rows.Next() {
		var user models.User
		if err := rows.Scan(&user.ID, &user.Name, &user.Surname, &user.Role); err != nil {
			return nil, err
		}
		users = append(users, user)
	}
	fmt.Println(users)
	return users, nil
}

func GetUserByNamePass(db *sql.DB, username string, password string) (models.User, error) {

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

func GetUserByID(db *sql.DB, userID int) (models.User, error) {
	rows, err := db.Query(`SELECT id, name, surname, role
	FROM public."Users" WHERE id = $1`, userID)

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
