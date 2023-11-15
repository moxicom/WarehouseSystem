package middleware

import (
	"APIServer/internal/db"
	"database/sql"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func UsersMW(dbase *sql.DB) gin.HandlerFunc {
	return func(c *gin.Context) {
		var data struct {
			SenderID int `json:"senderID"`
			UserID   int `json:"userID"`
		}

		if err := c.ShouldBindJSON(&data); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": "Неверный формат данных"})
			log.Println("ShouldBindJSON err")
			c.Abort()
			return
		}
		user, err := db.GetUserByID(dbase, data.SenderID)
		if err != nil {
			if err.Error() == "No rows" {
				c.JSON(http.StatusUnauthorized, gin.H{"error": err})
			} else {
				c.JSON(http.StatusNotFound, nil)
			}
		}
		userToProcess, err := db.GetUserByID(dbase, data.UserID)
		if err != nil {
			if err.Error() == "No rows" {
				c.JSON(http.StatusUnauthorized, gin.H{"process user error": err})
			} else {
				c.JSON(http.StatusNotFound, nil)
			}
		}

		c.Set("sender", user)
		c.Set("userToProcess", userToProcess)
		c.Next()
	}
}
