.DEFAULT_GOAL := scheduler

.PHONY: backend.dev
backend.dev:
	docker-compose run -d --service-ports --name backend.dev backend /bin/sleep infinity

.PHONY: backend.cli
backend.cli:
	docker exec -it backend.dev /bin/bash

.PHONY: frontend.dev
frontend.dev:
	docker-compose run -d --service-ports --name frontend.dev frontend /bin/sleep infinity

.PHONY: frontend.cli
backend.cli:
	docker exec -it frontend.dev /bin/bash

.PHONY: scheduler
scheduler:
	docker-compose up -d --build backend frontend

.PHONY: production
production:
	docker-compose up -d --build production

.PHONY: mongo-express
mongo-express:
	docker-compose up -d mongo-express

.PHONY: teardown
teardown:
	docker-compose down