package db

import (
	"APIServer/internal/models"
	"database/sql"
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
	// time.Sleep(1 * time.Second)
	return categories, nil
}

func InsertCategory(db *sql.DB, category models.Category, creatorID int) error {
	time.Sleep(50 * time.Millisecond)
	_, err := db.Exec(`INSERT INTO public."Categories"(title, creator_id) VALUES ($1, $2)`,
		category.Title, creatorID)
	if err != nil {
		return err
	}
	return nil
}

func UpdateCategory(db *sql.DB, category models.Category) error {
	time.Sleep(50 * time.Millisecond)
	_, err := db.Exec(`
		UPDATE public."Categories"
		SET title = $1
		WHERE id = $3
	`, category.Title, category.ID)
	if err != nil {
		return err
	}
	return nil
}

func GetCategoryTitle(db *sql.DB, categoryID int) (string, error) {
	rows, err := db.Query(`SELECT title FROM public."Categories" WHERE id = $1`, categoryID)
	if err == sql.ErrNoRows {
		return "", err
	} else if err != nil {
		return "", err
	}

	var title string

	for rows.Next() {
		if err := rows.Scan(&title); err != nil {
			return "", err
		}
	}

	return title, nil
}

func DeleteCategory(db *sql.DB, categoryID int) error {
	_, err := db.Query(`DELETE FROM public."Categories" WHERE id = $1`, categoryID)
	if err != nil {
		return err
	}
	return nil
}
