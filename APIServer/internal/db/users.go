package db

import (
	"APIServer/internal/models"
	"database/sql"
	"errors"
)

func GetAllUsers(db *sql.DB) ([]models.User, error) {
	rows, err := db.Query(`SELECT id, name, surname, username, password, role
	FROM public."Users"`)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	var users []models.User
	for rows.Next() {
		var user models.User
		if err := rows.Scan(&user.ID, &user.Name, &user.Surname, &user.Username, &user.Password, &user.Role); err != nil {
			return nil, err
		}
		users = append(users, user)
	}
	return users, nil
}

func GetUserByNamePass(db *sql.DB, username string, password string) (models.User, error) {

	rows, err := db.Query(`SELECT id, name, surname, username, password, role
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
		if err := rows.Scan(&user.ID, &user.Name, &user.Surname, &user.Username, &user.Password, &user.Role); err != nil {
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
	rows, err := db.Query(`SELECT id, name, surname, username, password, role
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
		if err := rows.Scan(&user.ID, &user.Name, &user.Surname, &user.Username, &user.Password, &user.Role); err != nil {
			return models.User{}, err
		}
		found = true
	}

	if !found {
		return models.User{}, errors.New("No rows")
	}

	return user, nil
}

func IsUserExist(db *sql.DB, username string) (bool, error) {
	query := `SELECT EXISTS(SELECT 1 FROM public."Users" WHERE username = $1)`
	var exists bool
	err := db.QueryRow(query, username).Scan(&exists)
	if err != nil {
		return false, err
	}
	return exists, nil
}

func DeleteUserByID(db *sql.DB, userID int) error {
	_, err := db.Exec(`DELETE FROM public."Users" WHERE id = $1`, userID)
	if err != nil {
		return err
	}
	return nil
}

func InsertUser(db *sql.DB, user models.User) error {
	_, err := db.Exec(`
		INSERT INTO public."Users"(name, surname, username, password, role) VALUES ($1, $2, $3, $4, $5)
	`, user.Name, user.Surname, user.Username, user.Password, user.Role)
	if err != nil {
		return err
	}
	return nil
}

func UpdateUser(db *sql.DB, user models.User) error {
	var query string
	var err error
	if user.Password == "" {
		query = "UPDATE public.\"Users\" SET name = $1, surname = $2, username = $3, role = $4 WHERE id = $5"
		_, err = db.Exec(query, user.Name, user.Surname, user.Username, user.Role, user.ID)
	} else {
		query = "UPDATE public.\"Users\" SET name = $1, surname = $2, username = $3, password = $4, role = $5 WHERE id = $6"
		_, err = db.Exec(query, user.Name, user.Surname, user.Username, user.Password, user.Role, user.ID)
	}
	return err
}
