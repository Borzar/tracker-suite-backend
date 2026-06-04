INSERT INTO categories (id, name)
VALUES
    (gen_random_uuid(), 'Work'),
    (gen_random_uuid(), 'Personal'),
    (gen_random_uuid(), 'Study');

INSERT INTO users (id, name, email)
VALUES
    (gen_random_uuid(), 'Boris', 'boris.zlobos@gmail.com');