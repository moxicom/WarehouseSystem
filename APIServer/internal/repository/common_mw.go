package repository

import (
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func (r *Repository) CommonMiddleware() gin.HandlerFunc {
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
		user, err := r.GetUserByID(i)
		if err != nil {
			if err.Error() == "no rows" {
				c.JSON(http.StatusUnauthorized, gin.H{"error": err.Error()})
				c.Abort()
				return
			} else {
				c.JSON(http.StatusNotFound, nil)
				c.Abort()
				return
			}
		}
		c.Set("user", user)
		c.Next()
	}
}
