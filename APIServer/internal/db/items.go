package db

import (
	"APIServer/internal/models"
	"database/sql"
	"fmt"
	"log"
	"time"
)

func GetAllItems(db *sql.DB, categoryID int) ([]models.Item, error) {
	rows, err := db.Query(`SELECT id, title, description, category_id, amount FROM public."Items" WHERE category_id = $1`, categoryID)

	fmt.Printf("Getting items from %d category\n", categoryID)

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

	time.Sleep(1 * time.Second)
	return items, nil
}
