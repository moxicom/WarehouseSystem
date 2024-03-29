package repository

import (
	"APIServer/internal/models"
	"database/sql"
	"log"
)

func (r Repository) GetAllItems(categoryID int) ([]models.Item, error) {
	rows, err := r.db.Query(`SELECT id, title, description, category_id, amount FROM public."items" WHERE category_id = $1`, categoryID)

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

func (r Repository) DeleteItem(itemID int) error {
	_, err := r.db.Exec(`DELETE FROM public."items" WHERE id = $1`, itemID)
	if err != nil {
		return err
	}
	return nil
}

func (r Repository) DeleteItemsByCategory(categoryID int) error {
	_, err := r.db.Exec(`DELETE FROM public."items" WHERE category_id = $1`, categoryID)
	if err != nil {
		return err
	}
	return nil
}

func (r Repository) InsertItem(item models.Item) error {
	_, err := r.db.Exec(`
		INSERT INTO public."items"(title, description, category_id, amount) 
		VALUES ($1, $2, $3, $4)`,
		item.Title, item.Description, item.CategoryID, item.Amount)
	if err != nil {
		return err
	}
	return nil
}

func (r Repository) UpdateItem(item models.Item) error {
	_, err := r.db.Exec(`
		UPDATE public."items"
		SET title=$1,
		description=$2,
		amount=$3
		WHERE id=$4;
	`, item.Title, item.Description, item.Amount, item.ID)
	if err != nil {
		return err
	}
	return nil
}
