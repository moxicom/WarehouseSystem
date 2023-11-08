package middleware

import (
	"APIServer/internal/db"
	"APIServer/internal/models"
	"database/sql"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func ItemMW(dbase *sql.DB) gin.HandlerFunc {
	return func(c *gin.Context) {
		var data struct {
			UserID int         `json:"userID"`
			Item   models.Item `json:"itemData"`
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

		c.Set("user", user)
		c.Set("item", data.Item)
		c.Next()
	}
}
