package middleware

import (
	"APIServer/internal/db"
	"database/sql"
	"encoding/json"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func DataMW[T any](dbase *sql.DB) gin.HandlerFunc {
	return func(c *gin.Context) {
		var data struct {
			UserID int             `json:"userID"`
			Data   json.RawMessage `json:"data"`
		}

		if err := c.ShouldBindJSON(&data); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": "Неверный формат данных"})
			log.Println("ShouldBindJSON err")
			c.Abort()
			return
		}
		i := data.UserID
		user, err := db.GetUserByID(dbase, i)

		if err != nil {
			if err.Error() == "No rows" {
				c.JSON(http.StatusUnauthorized, gin.H{"error": err})
			} else {
				c.JSON(http.StatusNotFound, nil)
			}
		}

		var itemData T
		if err := json.Unmarshal(data.Data, &itemData); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": "Невозможно разобрать данные"})
			log.Println("JSON Unmarshal error")
			c.Abort()
			return
		}

		c.Set("user", user)
		c.Set("data", itemData)
		c.Next()
	}
}
