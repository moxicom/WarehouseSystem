package db

import (
	"APIServer/internal/models"
	"database/sql"
	"fmt"
	"strconv"
)

func addItem(db *sql.DB, item models.Item) error {
	_, err := db.Exec(`INSERT INTO public."Items" (title, description, category_id, amount) VALUES ($1, $2, $3, $4)`, item.Title, item.Description, item.CategoryID, item.Amount)

	if err != nil {
		return err
	}

	return nil
}

func AddItemsByConsole(db *sql.DB) {
	var title string
	var description string
	var categoryID int
	var amount int

	fmt.Println("Введите следующее:")
	fmt.Print("\tTitle: ")
	fmt.Scan(&title)
	fmt.Print("\tDescription: ")
	fmt.Scan(&description)
	fmt.Print("\tCategoryID: ")
	fmt.Scan(&categoryID)
	fmt.Print("\tAmout: ")
	fmt.Scan(&amount)

	var repeat int

	fmt.Print("Сколько раз повторять: ")
	fmt.Scan(&repeat)

	for i := 0; i < repeat; i++ {
		item := models.Item{
			Title:       title + " " + strconv.Itoa(i),
			Description: description,
			CategoryID:  categoryID,
			Amount:      amount,
		}
		err := addItem(db, item)
		if err != nil {
			return
		}
		fmt.Printf("Добавлен %dй %v\n", i+1, item)
	}
}
