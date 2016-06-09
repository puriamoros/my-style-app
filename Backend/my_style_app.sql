-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 09-06-2016 a las 16:46:46
-- Versión del servidor: 10.1.13-MariaDB
-- Versión de PHP: 5.6.20

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `my_style_app`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `appointments`
--

DROP TABLE IF EXISTS `appointments`;
CREATE TABLE `appointments` (
  `id` int(11) NOT NULL,
  `idClient` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL,
  `idService` int(11) NOT NULL,
  `date` varchar(100) NOT NULL,
  `notes` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `appointments`
--

INSERT INTO `appointments` (`id`, `idClient`, `idEstablishment`, `idService`, `date`, `notes`) VALUES
(1232, 1, 1, 1, 'asdf', 'asdf');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `establishments`
--

DROP TABLE IF EXISTS `establishments`;
CREATE TABLE `establishments` (
  `id` int(11) NOT NULL,
  `name` varchar(200) NOT NULL,
  `address` text NOT NULL,
  `idEstablishmentType` int(11) NOT NULL,
  `idOwner` int(11) NOT NULL,
  `idProvince` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `establishment_types`
--

DROP TABLE IF EXISTS `establishment_types`;
CREATE TABLE `establishment_types` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `establishment_types`
--

INSERT INTO `establishment_types` (`id`, `name`) VALUES
(1, 'Peluquería'),
(2, 'Estética'),
(3, 'Peluquería y Estética');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `favourites`
--

DROP TABLE IF EXISTS `favourites`;
CREATE TABLE `favourites` (
  `id` int(11) NOT NULL,
  `idClient` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `offer`
--

DROP TABLE IF EXISTS `offer`;
CREATE TABLE `offer` (
  `idEstablishment` int(11) NOT NULL,
  `idService` int(11) NOT NULL,
  `price` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `provinces`
--

DROP TABLE IF EXISTS `provinces`;
CREATE TABLE `provinces` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `provinces`
--

INSERT INTO `provinces` (`id`, `name`) VALUES
(1, 'Álava'),
(2, 'Albacete'),
(3, 'Alicante'),
(4, 'Almería '),
(5, 'Asturias '),
(6, 'Ávila '),
(7, 'Badajoz '),
(8, 'Barcelona '),
(9, 'Burgos '),
(10, 'Cáceres '),
(11, 'Cádiz '),
(12, 'Cantabria '),
(13, 'Castellón '),
(14, 'Ciudad Real'),
(15, 'Córdoba '),
(16, 'Cuenca '),
(17, 'Gerona '),
(18, 'Granada '),
(19, 'Guadalajara '),
(20, 'Guipúzcoa '),
(21, 'Huelva '),
(22, 'Huesca '),
(23, 'Islas Baleares'),
(24, 'Jaén '),
(25, 'La Coruña'),
(26, 'La Rioja'),
(27, 'Las Palmas'),
(28, 'León '),
(29, 'Lérida '),
(30, 'Lugo '),
(31, 'Madrid '),
(32, 'Málaga '),
(33, 'Murcia '),
(34, 'Navarra '),
(35, 'Orense '),
(36, 'Palencia '),
(37, 'Pontevedra '),
(38, 'Salamanca '),
(39, 'Segovia '),
(40, 'Sevilla '),
(41, 'Soria'),
(42, 'Tarragona '),
(43, 'Tenerife '),
(44, 'Teruel '),
(45, 'Toledo '),
(46, 'Valencia '),
(47, 'Valladolid '),
(48, 'Vizcaya '),
(49, 'Zamora '),
(50, 'Zaragoza ');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `services`
--

DROP TABLE IF EXISTS `services`;
CREATE TABLE `services` (
  `id` int(11) NOT NULL,
  `idTranslation` int(11) NOT NULL,
  `idServiceCategory` int(11) NOT NULL,
  `duration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `services`
--

INSERT INTO `services` (`id`, `idTranslation`, `idServiceCategory`, `duration`) VALUES
(1, 12, 1, 30),
(2, 13, 1, 30),
(3, 14, 1, 45),
(4, 15, 1, 45),
(5, 16, 1, 60),
(6, 17, 1, 45),
(7, 18, 2, 30),
(8, 19, 2, 30),
(9, 20, 2, 30),
(10, 21, 2, 30),
(11, 22, 2, 15),
(12, 23, 2, 15),
(13, 24, 3, 60),
(14, 25, 3, 60),
(15, 26, 3, 60),
(16, 27, 3, 45),
(17, 28, 3, 45),
(18, 29, 3, 60),
(19, 30, 4, 90),
(20, 31, 4, 90),
(21, 32, 5, 45),
(22, 33, 6, 30),
(23, 34, 6, 45),
(24, 35, 6, 30),
(25, 36, 6, 30),
(26, 37, 6, 30),
(27, 38, 6, 30),
(28, 39, 6, 30),
(29, 40, 6, 30),
(30, 41, 7, 60),
(31, 42, 7, 60),
(32, 43, 7, 60),
(33, 44, 8, 45),
(34, 45, 8, 30),
(35, 46, 8, 90),
(36, 47, 8, 30),
(37, 48, 8, 45),
(38, 49, 8, 60),
(39, 50, 9, 60),
(40, 51, 9, 60),
(41, 52, 10, 60),
(42, 53, 10, 60),
(43, 54, 10, 60),
(44, 55, 11, 45),
(45, 56, 11, 45),
(46, 57, 11, 45);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `services_history`
--

DROP TABLE IF EXISTS `services_history`;
CREATE TABLE `services_history` (
  `idClient` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL,
  `idService` int(11) NOT NULL,
  `date` varchar(50) NOT NULL,
  `notes` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `service_categories`
--

DROP TABLE IF EXISTS `service_categories`;
CREATE TABLE `service_categories` (
  `id` int(11) NOT NULL,
  `idTranslation` int(11) NOT NULL,
  `idEstablishmentType` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `service_categories`
--

INSERT INTO `service_categories` (`id`, `idTranslation`, `idEstablishmentType`) VALUES
(1, 1, 1),
(2, 2, 1),
(3, 3, 1),
(4, 4, 1),
(5, 5, 1),
(6, 6, 2),
(7, 7, 2),
(8, 8, 2),
(9, 9, 2),
(10, 10, 2),
(11, 11, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `staff`
--

DROP TABLE IF EXISTS `staff`;
CREATE TABLE `staff` (
  `idUser` int(11) NOT NULL,
  `idEstablishment` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `translations`
--

DROP TABLE IF EXISTS `translations`;
CREATE TABLE `translations` (
  `id` int(11) NOT NULL,
  `en` text NOT NULL,
  `es` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `translations`
--

INSERT INTO `translations` (`id`, `en`, `es`) VALUES
(1, 'Hair Styles', 'Peluquería'),
(2, 'Haircuts', 'Cortes'),
(3, 'Colouring and streaks', 'Coloración y mechas'),
(4, 'Permanents', 'Permanentes'),
(5, 'Hair extensions', 'Extensiones'),
(6, 'Hair removal', 'Depilación'),
(7, 'Hands and feet', 'Belleza manos-pies'),
(8, 'Eyes', 'Belleza ojos'),
(9, 'Facial', 'Facial'),
(10, 'Facial laser', 'Láser facial'),
(11, 'Makeups', 'Maquillajes'),
(12, '', 'Peinar corto'),
(13, '', 'Peinar mediano'),
(14, '', 'Peinar largo'),
(15, '', 'Semirecogido'),
(16, '', 'Recogido'),
(17, '', 'Trenzados completos'),
(18, '', 'Corte señora'),
(19, '', 'Corte caballero'),
(20, '', 'Maquinilla'),
(21, '', 'Corte infantil'),
(22, '', 'Retoque flequillo'),
(23, '', 'Arreglo barba'),
(24, '', 'Coloración (Tinte o baño de color)'),
(25, '', 'Mechas enteras'),
(26, '', 'Mechas enteras bicolor'),
(27, '', '1/2 Mechas'),
(28, '', '1/2 Mechas bicolor'),
(29, '', 'Mechas californianas'),
(30, '', 'Alisado Japonés'),
(31, '', 'Moldeador o ahuecador'),
(32, '', 'Colocación extensiones de clip'),
(33, '', 'Medias piernas tibias o calientes'),
(34, '', 'Piernas enteras tibias o calientes'),
(35, '', 'Pecho o espalda'),
(36, '', 'Brazos'),
(37, '', 'Ingles'),
(38, '', 'Ingles brasileñas'),
(39, '', 'Axilas'),
(40, '', 'Labio superior'),
(41, '', 'Maquillado permanente de uñas manos o pies'),
(42, '', 'Manicura'),
(43, '', 'Pedicura'),
(44, '', 'Permanente de pestañas'),
(45, '', 'Tinte de pestañas'),
(46, '', 'Extensiones de pestañas'),
(47, '', 'Tinte de cejas'),
(48, '', 'Aplicación de pestañas postizas'),
(49, '', 'Permanente + Tinte de pestañas'),
(50, '', 'Higiene facial completa'),
(51, '', 'Exfoliación e hidratación facial'),
(52, '', 'Fotorejuvenecimiento'),
(53, '', 'Manchas faciales'),
(54, '', 'Acné'),
(55, '', 'Maquillaje social'),
(56, '', 'Maquillaje fiesta'),
(57, '', 'Maquillaje carnaval o fantasía');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `surname` varchar(100) NOT NULL,
  `email` varchar(200) NOT NULL,
  `password` varchar(100) NOT NULL,
  `apiKey` varchar(100) NOT NULL,
  `userType` int(11) NOT NULL,
  `phone` varchar(9) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `users`
--

INSERT INTO `users` (`id`, `name`, `surname`, `email`, `password`, `apiKey`, `userType`, `phone`) VALUES
(1, 'Helio', 'Huete', 'helio.huete@gmail.com', '$2y$10$g0v8y38VjKujbzPQCa9g.e2HeYt5fLAC5o1asOxoqmvN53GvbReg6', 'cc06c9f321e156c2468669728e2be8b8', 0, '0'),
(19, 'Puri', 'Amorós', 'puri.amoros@gmail.com', '$2y$10$/kXT7NWAulGrX31H/BNLmOQfFr41znROhIxlDDuKlA1wU6mWHINxO', '3bc23b47841173b7027d911bc055d113', 0, '0');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `appointments`
--
ALTER TABLE `appointments`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `establishments`
--
ALTER TABLE `establishments`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `establishment_types`
--
ALTER TABLE `establishment_types`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `favourites`
--
ALTER TABLE `favourites`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `offer`
--
ALTER TABLE `offer`
  ADD PRIMARY KEY (`idEstablishment`,`idService`);

--
-- Indices de la tabla `provinces`
--
ALTER TABLE `provinces`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `services`
--
ALTER TABLE `services`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `services_history`
--
ALTER TABLE `services_history`
  ADD PRIMARY KEY (`idClient`,`idEstablishment`,`idService`);

--
-- Indices de la tabla `service_categories`
--
ALTER TABLE `service_categories`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `staff`
--
ALTER TABLE `staff`
  ADD PRIMARY KEY (`idUser`,`idEstablishment`);

--
-- Indices de la tabla `translations`
--
ALTER TABLE `translations`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `userEmail` (`email`),
  ADD UNIQUE KEY `userApiKey` (`apiKey`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `appointments`
--
ALTER TABLE `appointments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT de la tabla `establishments`
--
ALTER TABLE `establishments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `establishment_types`
--
ALTER TABLE `establishment_types`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `favourites`
--
ALTER TABLE `favourites`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `provinces`
--
ALTER TABLE `provinces`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=51;
--
-- AUTO_INCREMENT de la tabla `services`
--
ALTER TABLE `services`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;
--
-- AUTO_INCREMENT de la tabla `service_categories`
--
ALTER TABLE `service_categories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
