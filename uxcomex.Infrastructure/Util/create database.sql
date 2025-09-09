create database uxcomex

use uxcomex

create table clientes (
cli_id uniqueidentifier not null default newid(),
cli_nome varchar(255) not null,
cli_email varchar(255) null,
cli_telefone varchar(15) null,
cli_data_cadastro datetime not null default getdate(),

    CONSTRAINT PK_Clientes PRIMARY KEY (cli_id),

)

create table produtos (
pro_id uniqueidentifier not null default newid(),
pro_nome varchar(255) not null,
pro_descricao text null,
pro_valor decimal(10,2) not null,
pro_quantidade_estoque decimal(10,2) not null

   CONSTRAINT PK_Produtos PRIMARY KEY (pro_id),
)

create table pedidos (
ped_id  uniqueidentifier not null default newid(),
ped_data datetime not null default getdate(),
ped_valor_total decimal(10,2) not null,
ped_status varchar(25),
cli_id uniqueidentifier not null,


    CONSTRAINT PK_Pedidos PRIMARY KEY (ped_id),
    CONSTRAINT FK_Pedidos_Cliente FOREIGN KEY (cli_id) REFERENCES clientes(cli_id) ON DELETE CASCADE,

)

create table item_pedidos (
itp_id  uniqueidentifier not null default newid(),
ped_id  uniqueidentifier not null default newid(),
pro_id uniqueidentifier not null default newid(),
itp_quantidade decimal(10,2) not null,
itp_preco_unitario decimal(10,2) not null

    CONSTRAINT PK_ItemPedidos PRIMARY KEY (itp_id),
    CONSTRAINT FK_ItemPedidos_Pedido FOREIGN KEY (ped_id) REFERENCES pedidos(ped_id) ON DELETE CASCADE,
    CONSTRAINT FK_ItemPedidos_Produto FOREIGN KEY (pro_id) REFERENCES produtos(pro_id),
    CONSTRAINT UK_ItemPedidos_PedidoProduto UNIQUE (ped_id, pro_id)

)

create table notificacao(
	not_id uniqueidentifier not null default newid(),
	ped_id uniqueidentifier not null,
	ped_status varchar(255) not null,
	not_data datetime not null default getdate()

    CONSTRAINT PK_Notificacao PRIMARY KEY (not_id),
    CONSTRAINT FK_Notificacao_Pedido FOREIGN KEY (ped_id) REFERENCES pedidos(ped_id) ON DELETE CASCADE,
)

CREATE NONCLUSTERED INDEX IX_Clientes_Nome ON clientes(cli_nome);
CREATE NONCLUSTERED INDEX IX_Clientes_Email ON clientes(cli_email);
CREATE NONCLUSTERED INDEX IX_Produtos_Nome ON produtos(pro_id);
CREATE NONCLUSTERED INDEX IX_Pedidos_Cliente ON pedidos(cli_id);
CREATE NONCLUSTERED INDEX IX_Pedidos_Data ON pedidos(ped_data);
CREATE NONCLUSTERED INDEX IX_Pedidos_Status ON pedidos(ped_status);
CREATE NONCLUSTERED INDEX IX_ItemPedidos_Pedido ON item_pedidos(ped_id);
CREATE NONCLUSTERED INDEX IX_ItemPedidos_Produto ON item_pedidos(pro_id);

-- ==============================================
-- Insert Sample Data
-- ==============================================
-- Clientes
INSERT INTO clientes (cli_nome, cli_email, cli_telefone)
VALUES
('Gabriel Silva', 'gabriel@example.com', '11999999999'),
('Maria Oliveira', 'maria@example.com', '11988888888'),
('João Santos', 'joao@example.com', '11977777777'),
('Ana Costa', 'ana@example.com', '11966666666'),
('Carlos Lima', 'carlos@example.com', '11955555555'),
('Fernanda Souza', 'fernanda@example.com', '11944444444'),
('Lucas Pereira', 'lucas@example.com', '11933333333'),
('Patricia Almeida', 'patricia@example.com', '11922222222'),
('Ricardo Fernandes', 'ricardo@example.com', '11911111111'),
('Sofia Mendes', 'sofia@example.com', '11900000000'),
('Diego Rocha', 'diego@example.com', '11999988877');

-- Produtos
INSERT INTO produtos (pro_nome, pro_descricao, pro_valor, pro_quantidade_estoque)
VALUES
('Camiseta', 'Camiseta 100% algodão', 50.00, 100),
('Calça Jeans', 'Calça jeans azul', 120.00, 50),
('Tênis Esportivo', 'Tênis para corrida', 200.00, 30),
('Jaqueta', 'Jaqueta de couro', 300.00, 20),
('Meias', 'Meias esportivas', 15.00, 200),
('Boné', 'Boné com aba curva', 40.00, 75),
('Camisa Polo', 'Camisa polo de algodão', 80.00, 150),
('Shorts', 'Shorts esportivos', 60.00, 120),
('Tênis Casual', 'Tênis para uso diário', 180.00, 40),
('Jaqueta Jeans', 'Jaqueta jeans azul', 250.00, 30),
('Mochila', 'Mochila escolar', 90.00, 80);

-- Pedidos
DECLARE @cli1 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Gabriel Silva');
DECLARE @cli2 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Maria Oliveira');
DECLARE @cli3 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Ana Costa');
DECLARE @cli4 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Carlos Lima');
DECLARE @cli5 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Fernanda Souza');
DECLARE @cli6 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Lucas Pereira');
DECLARE @cli7 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Patricia Almeida');
DECLARE @cli8 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Ricardo Fernandes');
DECLARE @cli9 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Sofia Mendes');
DECLARE @cli10 UNIQUEIDENTIFIER = (SELECT TOP 1 cli_id FROM clientes WHERE cli_nome = 'Diego Rocha');

INSERT INTO pedidos (ped_valor_total, ped_status, cli_id)
VALUES
(200.00, 'Pendente', @cli1),
(120.00, 'Concluído', @cli2),
(355.00, 'Pendente', @cli3),
(55.00, 'Concluído', @cli4),
(100.00, 'Pendente', @cli5),
(160.00, 'Pendente', @cli6),
(240.00, 'Concluído', @cli7),
(180.00, 'Pendente', @cli8),
(400.00, 'Pendente', @cli9),
(270.00, 'Concluído', @cli10);

-- Item_Pedidos
DECLARE @ped1 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli1);
DECLARE @ped2 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli2);
DECLARE @ped3 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli3);
DECLARE @ped4 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli4);
DECLARE @ped5 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli5);
DECLARE @ped6 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli6);
DECLARE @ped7 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli7);
DECLARE @ped8 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli8);
DECLARE @ped9 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli9);
DECLARE @ped10 UNIQUEIDENTIFIER = (SELECT TOP 1 ped_id FROM pedidos WHERE cli_id = @cli10);

DECLARE @prod1 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Camiseta');
DECLARE @prod2 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Calça Jeans');
DECLARE @prod3 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Tênis Esportivo');
DECLARE @prod4 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Jaqueta');
DECLARE @prod5 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Meias');
DECLARE @prod6 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Boné');
DECLARE @prod7 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Camisa Polo');
DECLARE @prod8 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Shorts');
DECLARE @prod9 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Tênis Casual');
DECLARE @prod10 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Jaqueta Jeans');
DECLARE @prod11 UNIQUEIDENTIFIER = (SELECT TOP 1 pro_id FROM produtos WHERE pro_nome = 'Mochila');

INSERT INTO item_pedidos (ped_id, pro_id, itp_quantidade, itp_preco_unitario)
VALUES
(@ped1, @prod1, 2, 50.00),
(@ped1, @prod3, 1, 200.00),
(@ped2, @prod2, 1, 120.00),
(@ped3, @prod4, 1, 300.00),
(@ped3, @prod5, 1, 15.00),
(@ped3, @prod6, 1, 40.00),
(@ped4, @prod5, 3, 15.00),
(@ped5, @prod6, 2, 40.00),
(@ped5, @prod1, 1, 50.00),
(@ped6, @prod7, 2, 80.00),
(@ped6, @prod8, 1, 60.00),
(@ped7, @prod9, 1, 180.00),
(@ped7, @prod10, 1, 250.00),
(@ped8, @prod7, 1, 80.00),
(@ped8, @prod9, 1, 180.00),
(@ped9, @prod10, 2, 250.00),
(@ped9, @prod11, 1, 90.00),
(@ped10, @prod7, 1, 80.00),
(@ped10, @prod8, 2, 60.00),
(@ped10, @prod11, 1, 90.00);

-- Notificações
INSERT INTO notificacao (ped_id, ped_status)
VALUES
(@ped1, 'Pendente'),
(@ped2, 'Concluído'),
(@ped3, 'Pendente'),
(@ped4, 'Concluído'),
(@ped5, 'Pendente'),
(@ped6, 'Pendente'),
(@ped7, 'Concluído'),
(@ped8, 'Pendente'),
(@ped9, 'Pendente'),
(@ped10, 'Concluído');