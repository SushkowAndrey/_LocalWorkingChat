create table table_users_chat
(
    id           int auto_increment
        primary key,
    name_user varchar(100) not null unique,
	password   text          not null
);

create table table_users_chat_online
(
    id   text not null,
    name_user varchar(100) not null unique,
	date_time   text          not null
);

create table table_message
(
    id   text not null,
	text_message   text          not null,
	sender   text          not null,
	recipient   text          not null,
	date_time   text          not null
);