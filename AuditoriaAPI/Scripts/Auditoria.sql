create database dbAuditoria;

create table tbTiposDocumentos
(
	id int identity(1,1) primary key ,
	Descricao Varchar(2000)
);

create table tbSubTiposDocumentos
(
	id int identity(1,1) primary key ,
	IdTipoDocumento int,
	Descricao Varchar(2000),
	
	CONSTRAINT fk_tbSubTiposDocumentos_tbTiposDocumentos
	FOREIGN KEY (IdTipoDocumento)
	REFERENCES tbTiposDocumentos(id)
);

create table tbAuditoria
(
	id int identity(1,1) primary key ,
	idLoja int,
	Nomeloja varchar(300),
	idPessoa int,
	NomePessoa varchar(300),
	Obs Varchar(2000),
	DataCadastro Datetime,
	idUsuario int 
);

create table tbAuditoriaItem
(
	id int identity(1,1) primary key ,
	idAuditoria int,
	idSubTiposDocumentos int,
	DocumentosDescricao varchar(300)
	
	CONSTRAINT fk_tbAuditoriaItem_tbAuditoria
	FOREIGN KEY (idAuditoria)
	REFERENCES tbAuditoria(id),
	
	CONSTRAINT fk_tbAuditoriaItem_tbSubTiposDocumentos
	FOREIGN KEY (idSubTiposDocumentos)
	REFERENCES tbSubTiposDocumentos(id)
);

create table tbUsuario
(
	ID int identity(1,1) primary key ,
	Nome Varchar(300),
	Email Varchar(350),
	Senha Varchar(15),
	DataCadastro Datetime,
	Deleted bit null
);

create table tbToken
(
	TokenID int identity(1,1) primary key ,
	TokenKey Varchar(200),
	IssuedOn Datetime,
	ExpiresOn Datetime,
	CreatedOn Datetime,
	idUsuario int,
	Deleted bit null
);

create table tbUsuarioToken
(
	UsuarioTokenId int identity(1,1) primary key ,
	TokenID int  ,
	UsuarioID int 

	CONSTRAINT fk_tbUsuarioToken_tbToken
	FOREIGN KEY (TokenID)
	REFERENCES tbToken(TokenID),
	
);