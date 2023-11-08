package db

import (
	"APIServer/internal/models"
	"database/sql"
	"errors"
	"log"
	"time"
)

func GetAllItems(db *sql.DB, categoryID int) ([]models.Item, error) {
	time.Sleep(50 * time.Millisecond)
	rows, err := db.Query(`SELECT id, title, description, category_id, amount FROM public."Items" WHERE category_id = $1`, categoryID)

	if err == sql.ErrNoRows {
		log.Println("sql.NoRows")
		return nil, err
	} else if err != nil {
		return nil, err
	}

	defer rows.Close()

	var items []models.Item

	for rows.Next() {
		var item models.Item
		if err := rows.Scan(&item.ID,
			&item.Title,
			&item.Description,
			&item.CategoryID,
			&item.Amount); err != nil {
			return nil, err
		}
		items = append(items, item)
	}
	return items, nil
}

func DeleteItem(db *sql.DB, itemID int) error {
	time.Sleep(50 * time.Millisecond)
	_, err := db.Exec(`DELETE FROM public."Items" WHERE id = $1`, itemID)

	if err != nil {
		return err
	}

	return nil
}

func InsertItem(db *sql.DB) error {
	time.Sleep(50 * time.Millisecond)
	return errors.New("asd")
}
