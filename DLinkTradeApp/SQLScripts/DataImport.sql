use productsdb;

insert users(Login, Pass, Email, Phone)
values
('root', 'testpass', 'test@mail.com', '41414141');


insert ProductTypes(TypeName)
values
('Коммутаторы'),
('Маршрутизаторы'),
('Межсетевые экраны'),
('Медиа конвертеры');

insert Storages(Address)
values
('Random Address, 123'),
('Fuckin Slavers, 51'),
('Stupid Makers,12/a'),
('Fuckers 1/a');


insert Products(ProductName, ProductType, Cost, ProductDescription, Image)
values
('D-link DMS-1100-10TP', 1, 40000, 'Настраиваемый L2 коммутатор с 8 портами 100/1000/2.5GBase-T и 2 портами 10GBase-X SFP+ (8 портов PoE 802.3af/at, PoE‑бюджет 240 Вт)', load_file('D:/data/Images/SwitchBoards/DMS-1100-10TP.jpg')),
('D-link DMS-1100-10TS', 1, 38000, 'Настраиваемый L2 коммутатор с 8 портами 100/1000/2.5GBase-T и 2 портами 10GBase-X SFP+', load_file('D:/data/Images/SwitchBoards/DMS-1100-10TS.jpg')),
('DIS-100E-5W', 1, 15000, 'Промышленный неуправляемый коммутатор с 5 портами 10/100Base-TX', load_file('D:/data/Images/SwitchBoards/DIS-100E-5W.jpg')),
('DSA-2006', 2, 6000, 'Сервисный маршрутизатор с 6 настраиваемыми портами', load_file('D:/data/Images/Routers/DSA-2006.jpg')),
('DIR-X1860', 2, 5066, 'Двухдиапазонный гигабитный Wi-Fi 6 маршрутизатор AX1800', load_file('D:/data/Images/Routers/DIR-X1860.jpg')),
('DIR-843', 2, 6100, 'Беспроводной двухдиапазонный гигабитный маршрутизатор AC1200 Wave 2 с поддержкой MU-MIMO', load_file('D:/data/Images/Routers/DIR-843.jpg')),
('DIR-825/GFRU', 2, 8123, 'Беспроводной двухдиапазонный гигабитный маршрутизатор AC1200 Wave 2 с оптическим WAN-портом, поддержкой MU-MIMO, 3G/LTE и USB-портом', load_file('D:/data/Images/Routers/DIR-825GFRU.jpg')),
('DFL-870', 3, 6410, 'Гигабитный межсетевой экран NetDefend с 6 настраиваемыми портами', load_file('D:/data/Images/Firewalls/DFL-870.jpg')),
('DMC-1000', 4, 9000, 'Шасси для медиаконвертеров с 16 слотами расширения', load_file('D:/data/Images/MediaConverters/DMC-1000.jpg')),
('DMC-1910T', 4, 5100, 'WDM медиаконвертер с 1 портом 1000Base-T и 1 портом 1000Base-LX с разъемом SC (ТХ: 1550 нм; RX: 1310 нм) для одномодового оптического кабеля (до 15 км)', load_file('D:/data/Images/MediaConverters/DMC-1910T.jpg')),
('DMC-700SC', 4, 8800, 'Медиаконвертер с 1 портом 1000Base-T и 1 портом 1000Base-SX с разъемом SC для многомодового оптического кабеля (до 550 м)', load_file('D:/data/Images/MediaConverters/DMC-700SC.jpg')),
('DMC-920T', 4, 5600, 'WDM медиаконвертер с 1 портом 10/100Base-TX и 1 портом 100Base-FX с разъемом SC (ТХ: 1550 нм; RX: 1310 нм) для одномодового оптического кабеля (до 20 км)', load_file('D:/data/Images/MediaConverters/DMC-920T.jpg'));