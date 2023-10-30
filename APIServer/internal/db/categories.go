package db

import (
	"APIServer/internal/models"
	"database/sql"
	"fmt"
	"time"
)

func GetAllCategories(db *sql.DB) ([]models.Category, error) {
	rows, err := db.Query(`SELECT id, title, creator_id, created_at FROM public."Categories"`)

	if err == sql.ErrNoRows {
		return nil, err
	} else if err != nil {
		return nil, err
	}

	defer rows.Close()

	var categories []models.Category

	for rows.Next() {
		var category models.Category
		if err := rows.Scan(&category.ID, &category.Title, &category.CreatorID, &category.CreatedAt); err != nil {
			return nil, err
		}
		categories = append(categories, category)
	}

	fmt.Println(categories)

	time.Sleep(1 * time.Second)
	return categories, nil
}
