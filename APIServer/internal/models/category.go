package models

import "time"

type Category struct {
	ID        int
	Title     string
	CreatorID int
	CreatedAt time.Time
}
