CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username TEXT NOT NULL
);

CREATE TABLE sources (
    id SERIAL PRIMARY KEY,
    heading TEXT,
    subname1 TEXT,
    subname2 TEXT,
    name TEXT
);

CREATE TABLE specifications (
    id SERIAL PRIMARY KEY,
    name TEXT,
    xml_file_path TEXT NOT NULL,
    is_public BOOLEAN,
    -- no longer need columns for details stored in the XML
    delimiter TEXT,
    is_first_row_heading BOOLEAN
);

CREATE TABLE data_sources (
    user_id INTEGER REFERENCES users(id),
    source_id INTEGER REFERENCES sources(id),
    specification_id INTEGER REFERENCES specifications(id),
    PRIMARY KEY (user_id, source_id, specification_id)
);

-- Constants table is optional, depending on how you want to manage them.
CREATE TABLE constants (
    id SERIAL PRIMARY KEY,
    update_path TEXT,
    index_path TEXT
);

CREATE TABLE icon_types (
    id SERIAL PRIMARY KEY,
    name TEXT,
    class_type TEXT  -- Assuming ClassType is another string representation
);