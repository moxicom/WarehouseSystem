package middleware

import (
	"APIServer/internal/db"
	"database/sql"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func CommonMiddleware(dbase *sql.DB) gin.HandlerFunc {
	return func(c *gin.Context) {
		var userData struct {
			UserID int `json:"userID"`
		}

		log.Println("Common middleware started")

		if err := c.ShouldBindJSON(&userData); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": "Неверный формат данных"})
			c.Abort()
			return
		}

		i := userData.UserID
		user, err := db.GetUserByID(dbase, i)

		if err != nil {
			if err.Error() == "No rows" {
				c.JSON(http.StatusUnauthorized, gin.H{"error": err})
			} else {
				c.JSON(http.StatusNotFound, nil)
			}
		}
		c.Set("user", user)
		c.Next()
	}
}
