package middleware

import (
	"log"
	"net/http"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/golang-jwt/jwt"
)

func LoginMiddleware(secretkey string) gin.HandlerFunc {
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

		// Проверка данных пользователя в базе данных
		user, password := authData.Username, authData.Password

		claims := jwt.MapClaims{
			"user_id": 1233,
			"exp":     time.Now().Add(time.Hour * 1).Unix(), // Срок действия токена (1 час)
		}

		token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
		tokenString, err := token.SignedString([]byte(secretkey))
		if err != nil {
			c.JSON(http.StatusUnauthorized, gin.H{"error": "Недействительный токен"})
			log.Println("SignedString err")
			c.Abort()
			return
		}

		log.Println(user, password, tokenString)

		c.Set("user", user)         // Добавьте информацию о пользователе в контекст
		c.Set("password", password) // Добавление пароля
		c.Set("token", tokenString) // Добавьте JWT в контекст
		c.Next()
	}
}
