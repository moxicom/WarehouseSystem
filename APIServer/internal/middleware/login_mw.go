package middleware

import (
	"APIServer/internal/db"
	"database/sql"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func LoginMiddleware(dbase *sql.DB) gin.HandlerFunc {
	return func(c *gin.Context) {
		var authData struct {
			Username string `json:"username"`
			Password string `json:"password"`
		}

		log.Println("Middleware started")

		if err := c.ShouldBindJSON(&authData); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": "Неверный формат данных"})
			log.Println("ShouldBindJSON err")
			c.Abort()
			return
		}

		username, password := authData.Username, authData.Password
		log.Println(username, password)

		user, err := db.GetUserByNamePass(dbase, username, password)
		if err != nil {
			if err.Error() == "No rows" {
				c.JSON(http.StatusUnauthorized, gin.H{"error": err})
			} else {
				c.JSON(http.StatusNotFound, nil)
			}
			log.Println("error GetUser:", err)

			c.Abort()
		}
		c.Set("user", user)
		c.Next()
	}
}

// MAKES JWT ANSWER
// claims := jwt.MapClaims{
// 	"user_id": 1233,
// 	"exp":     time.Now().Add(time.Hour * 1).Unix(), // Token time
// }

// token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
// tokenString, err := token.SignedString([]byte(secretkey))
// if err != nil {
// 	c.JSON(http.StatusUnauthorized, gin.H{"error": "Недействительный токен"})
// 	log.Println("SignedString err")
// 	c.Abort()
// 	return
// }

// log.Println(user, password, tokenString)
// c.Set("token", tokenString)
