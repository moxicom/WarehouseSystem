package repository

import (
	"APIServer/internal/models"
	"database/sql"
	"time"
)

func (r *Repository) GetAllCategories() ([]models.Category, error) {
	rows, err := r.db.Query(`SELECT id, title, creator_id, created_at FROM public."categories"`)
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

func (r *Repository) InsertCategory(category models.Category, creatorID int) error {
	_, err := r.db.Exec(`INSERT INTO public."categories"(title, creator_id, created_at) VALUES ($1, $2, $3)`,
		category.Title, creatorID, time.Now())
	if err != nil {
		return err
	}
	return nil
}

func (r *Repository) UpdateCategory(category models.Category) error {
	_, err := r.db.Exec(`
		UPDATE public."categories"
		SET title=$1
		WHERE id=$2;
	`, category.Title, category.ID)
	if err != nil {
		return err
	}
	return nil
}

func (r *Repository) GetCategoryTitle(categoryID int) (string, error) {
	rows, err := r.db.Query(`SELECT title FROM public."categories" WHERE id = $1`, categoryID)
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

func (r *Repository) DeleteCategory(categoryID int) error {
	_, err := r.db.Query(`DELETE FROM public."categories" WHERE id = $1`, categoryID)
	if err != nil {
		return err
	}
	return nil
}
