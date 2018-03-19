Create dataBase ControleCtDB
go

use ControleCtDB

go
Create Table Pessoas
(
	Id				int				identity(1,1) not null,
	Nome			varchar(150)	not null,
	Cpf_Cnpj		varchar(18)		not null,
	NomeFantasia	varchar(150)	null,
	DataNascimento	smalldatetime	null,
	TipoPessoa		char(2)			not null check(TipoPessoa IN('PJ','PF')),	
	constraint PK_Pessoas primary key clustered(Id)
)
go

Create table Contas
(
	Id				int				not null,
	Nome			varchar(150)	not null,
	DataCriacao		smalldatetime	not null,
	IdCtPai			int				not null,
	Saldo			money			not null,	
	Situacao		varchar(9)		not null check(Situacao IN('Ativa','Bloqueada','Cancelada')),
	IdPessoa		int				not null,
	ContaMatriz	    bit				not null,
	constraint PK_Contas primary key clustered(Id),
	constraint FK_ContasPessoas foreign Key (IdPessoa) references Pessoas(Id)
	on delete cascade
	on update cascade
)
go


create table Transferencias
(
	Id				bigint			identity(1,1) not null,
	IdContaOrigem	int				not null,
	IdContaDestino	int				not null,
	Valor			money			not null,	
	DataTransacao 	smalldatetime	null,
	Estornada		bit				not null -- 1 para transferencias estornadas
	
	constraint PK_Transferencias primary key clustered(Id),
	constraint FK_TransferenciasPessoasD foreign Key (IdContaDestino) references Contas(Id),
	constraint FK_TransferenciasPessoasO foreign Key (IdContaOrigem) references Contas(Id)
)

go
create table Aportes
(
	IdAlpha			varchar(15)		not null,
	IdContaDestino	int				not null,
	Valor			money			not null,	
	DataTransacao 	smalldatetime	null,
	Estornada		Bit				not null -- 1 para transferencias estornadas
	
	constraint PK_Aportes primary key clustered(IdAlpha),
	constraint FK_AportesPessoa foreign Key (IdContaDestino) references Contas(Id)
	on delete cascade
	on update cascade
)

go

create table ArvoreConta
(
	IdOrigem		int			not null,
	IdPaiOrigem		int			null,
	IdDestino		int			not null,
	IdPaiDestino	int			null		
	constraint FK_IdOrigem foreign Key (IdOrigem) references Contas(Id),
	constraint FK_IdDestino foreign Key (IdDestino) references Contas(Id)
)