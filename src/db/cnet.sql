/*
 Navicat Premium Data Transfer

 Source Server         : localhostMysql
 Source Server Type    : MySQL
 Source Server Version : 80028
 Source Host           : localhost:3306
 Source Schema         : cnet

 Target Server Type    : MySQL
 Target Server Version : 80028
 File Encoding         : 65001

 Date: 09/03/2023 22:01:44
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for pub_department
-- ----------------------------
DROP TABLE IF EXISTS `pub_department`;
CREATE TABLE `pub_department`  (
  `DeptCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门编号',
  `DeptName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门名称',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `ParentCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '上级部门编号',
  `DeptLevel` int(0) NOT NULL COMMENT '部门级别',
  `Lmid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '最后编辑人',
  `Lmdt` datetime(0) NULL DEFAULT NULL COMMENT '最后编辑时间',
  `StopFlag` tinyint(0) NULL DEFAULT NULL COMMENT '停用状态 默认0 未停用 1 停用',
  PRIMARY KEY (`DeptCode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_department
-- ----------------------------
INSERT INTO `pub_department` VALUES ('D000001', '总部', NULL, '', 0, '-', '2018-05-11 17:35:06', 0);
INSERT INTO `pub_department` VALUES ('D000002', 'cts', '1111', 'D000001', 0, '00000002-admin', '2019-08-09 15:37:16', 0);
INSERT INTO `pub_department` VALUES ('D000003', 'It部', '22331', 'D000002', 0, '00000002-admin', '2019-10-08 09:44:16', 0);
INSERT INTO `pub_department` VALUES ('D000004', 'CRM', NULL, 'D000002', 0, '00000002-admin', '2019-08-09 15:37:29', 0);
INSERT INTO `pub_department` VALUES ('D000005', '博华', NULL, 'D000001', 0, '-', '2018-05-12 11:35:13', 0);
INSERT INTO `pub_department` VALUES ('D000008', '订单', '搜索ss', 'D000001', 0, '-', '2018-04-19 15:47:06', 1);
INSERT INTO `pub_department` VALUES ('D000009', '搜索', NULL, 'D000001', 0, '-', '2018-04-16 09:48:29', 1);
INSERT INTO `pub_department` VALUES ('D000011', '试试', NULL, 'D000001', 0, '-', '2018-04-16 09:55:50', 1);
INSERT INTO `pub_department` VALUES ('D000012', '订单', NULL, 'D000003', 0, '-', '2018-04-19 13:31:58', 1);
INSERT INTO `pub_department` VALUES ('D000013', '订单', NULL, 'D000001', 0, '-', '2018-04-19 13:34:18', 1);
INSERT INTO `pub_department` VALUES ('D000014', '是是是', NULL, 'D000001', 0, '-', '2018-04-19 13:34:22', 1);
INSERT INTO `pub_department` VALUES ('D000015', 'dd', NULL, 'D000003', 0, '-', '2018-04-19 14:47:37', 1);
INSERT INTO `pub_department` VALUES ('D000016', '1232', '123试试', 'D000003', 0, '-', '2018-04-19 15:23:51', 1);
INSERT INTO `pub_department` VALUES ('D000017', 'ss', 'ff', 'D000001', 0, '-', '2018-04-20 13:15:47', 1);
INSERT INTO `pub_department` VALUES ('D000018', 'sss', 'ddd', 'D000002', 0, '-', '2018-04-20 13:23:06', 1);
INSERT INTO `pub_department` VALUES ('D000019', 'sssfsdf', NULL, 'D000002', 0, '-', '2018-04-20 13:36:28', 1);
INSERT INTO `pub_department` VALUES ('D000020', 'ss', 'dd', '', 0, '-', '2018-04-23 13:17:30', 1);
INSERT INTO `pub_department` VALUES ('D000021', 'ss4', 'dd', 'D000001', 0, '-', '2018-05-12 10:06:20', 1);
INSERT INTO `pub_department` VALUES ('D000022', '试试', NULL, 'D000005', 0, '-', '2018-05-12 09:22:33', 0);
INSERT INTO `pub_department` VALUES ('D000023', 'tt2', '试试', 'D000001', 0, '-', '2018-05-12 09:24:56', 0);
INSERT INTO `pub_department` VALUES ('D000024', '订单', '搜索', 'D000001', 0, '-', '2018-04-27 10:02:32', 1);
INSERT INTO `pub_department` VALUES ('D000025', '是是是', '订单', 'D000001', 0, '-', '2018-04-27 10:02:55', 1);
INSERT INTO `pub_department` VALUES ('D000026', '搜索3', 's', 'D000001', 0, '-', '2018-05-12 09:20:14', 0);
INSERT INTO `pub_department` VALUES ('D000027', 'dd', '1111', 'D000001', 0, '-', '2018-04-27 10:05:01', 1);
INSERT INTO `pub_department` VALUES ('D000028', 'ss', NULL, 'D000001', 0, '-', '2018-04-27 10:07:46', 1);
INSERT INTO `pub_department` VALUES ('D000029', '孙菲菲', NULL, 'D000001', 0, '-', '2018-04-27 10:08:11', 1);
INSERT INTO `pub_department` VALUES ('D000030', 'd1', NULL, 'D000001', 0, '-', '2018-04-27 10:13:25', 1);
INSERT INTO `pub_department` VALUES ('D000031', 'ss', '11', 'D000003', 0, '-', '2018-04-27 10:13:38', 1);
INSERT INTO `pub_department` VALUES ('D000032', '111', NULL, 'D000001', 0, '-', '2018-04-27 10:13:49', 1);
INSERT INTO `pub_department` VALUES ('D000033', 'ss2', 'dd', 'D000023', 0, '-', '2018-05-12 11:35:19', 0);
INSERT INTO `pub_department` VALUES ('D000034', 'fff', 'ss', 'D000023', 0, '-', '2018-05-12 09:28:39', 0);
INSERT INTO `pub_department` VALUES ('D000035', 'fsss', 'ddd', 'D000023', 0, '-', '2018-05-11 14:46:05', 1);
INSERT INTO `pub_department` VALUES ('D000036', 'sf', 'ss', 'D000023', 0, '-', '2018-05-11 14:54:34', 1);
INSERT INTO `pub_department` VALUES ('D000037', 'ss1', '1', 'D000003', 0, '00000002-admin', '2019-08-09 15:37:06', 0);
INSERT INTO `pub_department` VALUES ('D000038', 'ss', '111', 'D000003', 0, '-', '2018-05-11 17:16:11', 1);
INSERT INTO `pub_department` VALUES ('D000039', '111', NULL, 'D000003', 0, '-', '2018-05-12 10:16:00', 1);
INSERT INTO `pub_department` VALUES ('D000040', '112', '22', 'D000023', 0, '00000002-admin', '2018-07-02 17:18:34', 1);
INSERT INTO `pub_department` VALUES ('D000041', '11', '22', 'D000026', 0, '00000002-admin', '2018-07-02 17:23:57', 0);
INSERT INTO `pub_department` VALUES ('D000042', '11', '22', 'D000026', 0, '00000002-admin', '2018-08-17 11:08:50', 0);
INSERT INTO `pub_department` VALUES ('D000043', '1122', '22', 'D000026', 0, '00000002-admin', '2018-08-17 11:09:31', 0);
INSERT INTO `pub_department` VALUES ('D000044', '12233', NULL, 'D000026', 0, '00000002-admin', '2018-08-17 11:11:00', 1);
INSERT INTO `pub_department` VALUES ('D000045', '11', '33', 'D000026', 0, '00000002-admin', '2018-08-17 11:12:50', 1);
INSERT INTO `pub_department` VALUES ('D000046', '2', '333', 'D000026', 0, '00000002-admin', '2018-08-17 11:13:42', 1);
INSERT INTO `pub_department` VALUES ('D000047', '112', '222', 'D000003', 0, '00000002-admin', '2019-08-09 14:06:16', 1);
INSERT INTO `pub_department` VALUES ('D000048', '123', '11', 'D000002', 0, '00000002-admin', '2019-08-09 15:19:14', NULL);
INSERT INTO `pub_department` VALUES ('D000049', '123', '11', 'D000002', 0, '00000002-admin', '2019-08-09 15:19:29', NULL);
INSERT INTO `pub_department` VALUES ('D000050', '122', '111', 'D000003', 0, '00000002-admin', '2019-08-09 15:20:18', NULL);
INSERT INTO `pub_department` VALUES ('D000051', '111', NULL, 'D000002', 0, '00000002-admin', '2019-08-09 15:26:12', 1);
INSERT INTO `pub_department` VALUES ('D000052', '121', NULL, 'D000002', 0, '00000002-admin', '2019-08-09 15:37:45', 1);

-- ----------------------------
-- Table structure for pub_function
-- ----------------------------
DROP TABLE IF EXISTS `pub_function`;
CREATE TABLE `pub_function`  (
  `FunctionCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '权限编号',
  `FunctionEnglish` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '英文编号',
  `FunctionChina` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '中文名称',
  `FunctionDescrip` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '说明',
  `ParentCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '父节点',
  `MenuFlag` tinyint(0) NOT NULL COMMENT '是否为菜单 1菜单 0权限',
  `StopFlag` tinyint(0) NULL DEFAULT NULL COMMENT '是否停用',
  `URLString` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '组件路径',
  `editdate` datetime(0) NULL DEFAULT NULL COMMENT '创建或修改时间',
  `editor` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '修改人 名字加工号 张三(000001)',
  `sortidx` int(0) NULL DEFAULT NULL COMMENT '排序字段',
  `target` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'navTab 嵌套  _blank 新窗口 dialog 弹出窗',
  `MenuIcon` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '菜单图标class',
  `RouterPath` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '路由路径',
  `IsCache` tinyint(0) NULL DEFAULT NULL COMMENT '是否组件缓存',
  PRIMARY KEY (`FunctionCode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_function
-- ----------------------------
INSERT INTO `pub_function` VALUES ('FC001', 'BASEINFO', '基础信息', NULL, '0', 1, 0, 'Main', '2018-04-04 00:00:00', NULL, 1, 'navTab', 'logo-buffer', '/baseSet', 0);
INSERT INTO `pub_function` VALUES ('FC001001', 'USERINFO', '用户信息', '12222', 'FC001', 1, 0, 'view/User/List.vue', '2019-10-12 15:06:53', '00000002-admin', 1, NULL, 'md-arrow-dropdown-circle', 'userManage', 1);
INSERT INTO `pub_function` VALUES ('FC001001001', 'USERINFOLIST', '查看', NULL, 'FC001001', 0, 0, NULL, '2019-10-10 11:04:31', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001001002', 'USERINFOADD', '添加', NULL, 'FC001001', 0, 0, NULL, '2018-05-14 13:41:27', '-', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001001003', 'USERINFOEDIT', '编辑', NULL, 'FC001001', 0, 0, NULL, '2018-05-14 13:42:06', '-', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001001004', 'USERINFOREMOVE', '删除', NULL, 'FC001001', 0, 0, NULL, '2018-05-14 13:45:13', '-', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001001005', 'USERINFOAUTH', '授权', NULL, 'FC001001', 0, 0, NULL, '2018-05-14 13:48:11', '-', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001002', 'ROLEINFO', '角色信息', NULL, 'FC001', 1, 0, 'view/Role/List.vue', '2022-04-21 15:08:49', '00000002-admin', 2, 'navTab', 'md-trending-up', 'roleManage', 0);
INSERT INTO `pub_function` VALUES ('FC001002001', 'ROLEINFOLIST', '查看', NULL, 'FC001002', 0, 0, NULL, '2018-05-15 13:09:51', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001002002', 'ROLEINFOADD', '添加', NULL, 'FC001002', 0, 0, NULL, '2018-05-15 13:10:28', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001002003', 'ROLEINFOEDIT', '编辑', NULL, 'FC001002', 0, 0, NULL, '2018-05-15 13:11:29', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001002004', 'ROLEINFOREMOVE', '删除', NULL, 'FC001002', 0, 0, NULL, '2018-05-15 13:12:04', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001002005', 'ROLEINFOAUTH', '授权', NULL, 'FC001002', 0, 0, NULL, '2019-10-08 09:44:05', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001003', 'FUNCTION', '权限信息', NULL, 'FC001', 1, 0, 'view/Permission/List.vue', '2018-05-15 13:18:40', '00000002-admin', 3, NULL, 'ios-infinite', 'functionManage', 0);
INSERT INTO `pub_function` VALUES ('FC001003001', 'FUNCTIONLIST', '查看', NULL, 'FC001003', 0, 0, NULL, '2018-05-15 13:20:01', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001003002', 'FUNCTIONADD', '添加', NULL, 'FC001003', 0, 0, NULL, '2018-05-15 13:20:21', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001003003', 'FUNCTIONEDIT', '编辑', NULL, 'FC001003', 0, 0, NULL, '2018-05-15 13:20:39', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001003004', 'FUNCTIONREMOVE', '删除', NULL, 'FC001003', 0, 0, NULL, '2018-05-15 13:21:02', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001004', 'DEPARTMENT', '组织信息', NULL, 'FC001', 1, 0, 'view/Dept/List.vue', '2018-05-15 13:22:18', '00000002-admin', 4, NULL, 'ios-people', 'companyManage', 0);
INSERT INTO `pub_function` VALUES ('FC001004001', 'DEPARTMENTLIST', '查看', NULL, 'FC001004', 0, 0, NULL, '2018-05-15 13:22:40', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001004002', 'DEPARTMENTADD', '添加', NULL, 'FC001004', 0, 0, NULL, '2018-05-15 13:23:00', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001004003', 'DEPARTMENTEDIT', '编辑', NULL, 'FC001004', 0, 0, NULL, '2018-05-15 13:23:17', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001004004', 'DEPARTMENTREMOVE', '删除', NULL, 'FC001004', 0, 0, NULL, '2018-05-15 13:23:36', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC001005', 'isnew_EchartTest', 'Echart', NULL, 'FC001', 1, 0, 'view/EchartTest/index1.vue', '2023-03-08 23:21:45', '00000002-admin', 9999, NULL, NULL, 'EchartTest', 0);
INSERT INTO `pub_function` VALUES ('FC002', 'BUSINESSINFO', '业务信息', NULL, '0', 1, 1, NULL, '2018-04-04 11:36:55', NULL, 2, 'navTab', NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC002001', 'test', '测试', NULL, 'FC002', 0, 1, NULL, '2018-06-29 16:44:39', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC002002', '222', 'tt1', NULL, 'FC002', 0, 1, NULL, '2018-07-02 17:19:05', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC002003', '1', '11', '5', '0', 0, 1, '2', '2019-08-13 16:04:46', '00000002-admin', 3, NULL, '4', NULL, 0);
INSERT INTO `pub_function` VALUES ('FC002004', 'test11', 'test2', NULL, '0', 1, 1, '111', '2019-08-13 16:28:56', '00000002-admin', 99, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC002004001', '222', '11', NULL, 'FC002004', 1, 0, '111', '2019-08-13 16:29:17', '00000002-admin', 22, NULL, NULL, NULL, 0);
INSERT INTO `pub_function` VALUES ('FC002005', 'test2', 'test2', NULL, 'FC002', 0, 1, NULL, '2018-08-17 11:15:11', '00000002-admin', 99, NULL, NULL, NULL, 0);

-- ----------------------------
-- Table structure for pub_role
-- ----------------------------
DROP TABLE IF EXISTS `pub_role`;
CREATE TABLE `pub_role`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `RoleCode` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色编号',
  `RoleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色名称',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `StopFlag` tinyint(0) NOT NULL COMMENT '停用状态 默认0  未停用 1 停用',
  `Crid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '创建人',
  `Crdt` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Lmid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '最后更新人',
  `Lmdt` datetime(0) NULL DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_role
-- ----------------------------
INSERT INTO `pub_role` VALUES (1, 'RC000001', '超级管理员', NULL, 0, '00000001', '2018-04-04 00:00:00', '00000001', '2018-04-04 00:00:00');
INSERT INTO `pub_role` VALUES (2, 'RC000002', '普通管理员', '2221', 0, NULL, NULL, '00000002-admin', '2022-04-12 13:17:26');
INSERT INTO `pub_role` VALUES (3, 'RC000003', '客服经理', '1111221', 0, NULL, NULL, '00000002-admin', '2019-08-14 10:10:38');
INSERT INTO `pub_role` VALUES (4, '000004', '销售经理', '12345', 1, '-', '2018-05-10 17:27:24', '-', '2018-05-10 17:27:24');
INSERT INTO `pub_role` VALUES (5, '000005', '客服专员', NULL, 1, '-', '2018-05-11 09:45:58', '-', '2018-05-11 09:45:58');
INSERT INTO `pub_role` VALUES (7, 'RC000007', 'tt', 'ss', 1, '-', '2018-05-11 16:27:18', '-', '2018-05-11 16:27:18');
INSERT INTO `pub_role` VALUES (8, 'RC000008', 'tt', '111', 1, '-', '2018-05-14 10:50:43', '-', '2018-05-14 10:50:43');
INSERT INTO `pub_role` VALUES (9, 'RC000009', '角色2', '2222', 1, NULL, NULL, '00000002-admin', '2019-08-14 10:10:53');
INSERT INTO `pub_role` VALUES (10, 'RC000010', '客户专员', NULL, 0, NULL, NULL, '00000002-admin', '2019-08-14 14:49:06');

-- ----------------------------
-- Table structure for pub_rolefunction
-- ----------------------------
DROP TABLE IF EXISTS `pub_rolefunction`;
CREATE TABLE `pub_rolefunction`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `RoleCode` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色编号',
  `FunctionCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '权限编号',
  `Lmid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '最后编辑人',
  `Lmdt` datetime(0) NULL DEFAULT NULL COMMENT '最后编辑时间',
  `StopFlag` tinyint(0) NULL DEFAULT NULL COMMENT '停用状态 默认0 未停用 1 停用',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 930 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_rolefunction
-- ----------------------------
INSERT INTO `pub_rolefunction` VALUES (1, '111', 'dddd', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (389, 'RC000006', 'FC001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (390, 'RC000006', 'FC001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (391, 'RC000006', 'FC001001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (562, 'RC000006', 'FC001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (563, 'RC000006', 'FC001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (564, 'RC000006', 'FC001001005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (565, 'RC000006', 'FC001001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (566, 'RC000006', 'FC001001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (567, 'RC000006', 'FC001001003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (568, 'RC000006', 'FC001001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (569, 'RC000006', 'FC001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (570, 'RC000006', 'FC001002001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (571, 'RC000006', 'FC001002002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (572, 'RC000006', 'FC001002003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (573, 'RC000006', 'FC001002004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (574, 'RC000006', 'FC001002005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (575, 'RC000006', 'FC001003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (576, 'RC000006', 'FC001003001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (577, 'RC000006', 'FC001003002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (578, 'RC000006', 'FC001003003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (579, 'RC000006', 'FC001003004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (580, 'RC000006', 'FC001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (581, 'RC000006', 'FC001004001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (582, 'RC000006', 'FC001004002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (583, 'RC000006', 'FC001004003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (584, 'RC000006', 'FC001004004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (585, 'RC000006', 'FC002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (586, 'RC000006', '0004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (594, 'RC000003', 'FC001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (595, 'RC000003', 'FC001001005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (596, 'RC000003', 'FC001001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (597, 'RC000003', 'FC001001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (598, 'RC000003', 'FC001001003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (599, 'RC000003', 'FC001001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (600, 'RC000003', 'FC001002001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (601, 'RC000003', 'FC001002002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (602, 'RC000010', 'FC001001005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (603, 'RC000010', 'FC001001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (873, 'RC000002', 'FC001001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (874, 'RC000002', 'FC001001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (875, 'RC000002', 'FC001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (876, 'RC000002', 'FC001002001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (877, 'RC000002', 'FC001002002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (878, 'RC000002', 'FC001002003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (879, 'RC000002', 'FC001002004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (880, 'RC000002', 'FC001002005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (881, 'RC000002', 'FC001003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (882, 'RC000002', 'FC001003001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (883, 'RC000002', 'FC001003002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (884, 'RC000002', 'FC001003003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (885, 'RC000002', 'FC001003004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (886, 'RC000002', 'FC001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (887, 'RC000002', 'FC001004001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (888, 'RC000002', 'FC001004002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (889, 'RC000002', 'FC001004003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (890, 'RC000002', 'FC001004004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (908, 'RC000001', 'FC001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (909, 'RC000001', 'FC001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (910, 'RC000001', 'FC001001001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (911, 'RC000001', 'FC001001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (912, 'RC000001', 'FC001001003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (913, 'RC000001', 'FC001001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (914, 'RC000001', 'FC001001005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (915, 'RC000001', 'FC001002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (916, 'RC000001', 'FC001002001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (917, 'RC000001', 'FC001002002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (918, 'RC000001', 'FC001002003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (919, 'RC000001', 'FC001002004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (920, 'RC000001', 'FC001002005', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (921, 'RC000001', 'FC001003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (922, 'RC000001', 'FC001003001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (923, 'RC000001', 'FC001003002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (924, 'RC000001', 'FC001003003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (925, 'RC000001', 'FC001003004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (926, 'RC000001', 'FC001004', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (927, 'RC000001', 'FC001004001', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (928, 'RC000001', 'FC001004002', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (929, 'RC000001', 'FC001004003', NULL, NULL, NULL);
INSERT INTO `pub_rolefunction` VALUES (930, 'RC000001', 'FC001004004', NULL, NULL, NULL);

-- ----------------------------
-- Table structure for pub_user
-- ----------------------------
DROP TABLE IF EXISTS `pub_user`;
CREATE TABLE `pub_user`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `UserCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '登录用户名',
  `UserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '昵称/用户名',
  `RealName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '真实姓名',
  `UserPwd` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '登录密码',
  `Sex` tinyint(0) NOT NULL COMMENT '性别',
  `IdentityNo` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '身份证号码',
  `Birthday` datetime(0) NULL DEFAULT NULL COMMENT '生日',
  `DeptCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '部门编号',
  `ManagerFlag` tinyint(0) NOT NULL COMMENT '是否是管理员 默认不是 0  是1',
  `Tel` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '电话',
  `EMail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '邮箱',
  `QQ` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'QQ',
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `StopFlag` tinyint(0) NOT NULL COMMENT '停用状态 默认0 未停用 1停用',
  `Crid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '创建人',
  `Crdt` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Lmid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '最后更新人',
  `Lmdt` datetime(0) NULL DEFAULT NULL COMMENT '最后更新时间',
  `LoginDate` datetime(0) NULL DEFAULT NULL COMMENT '最后登录时间',
  `ProvinceCode` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '省份编号',
  `CityCode` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '城市编号',
  `RegionCode` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '区域编号',
  `UserAddress` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '地址',
  `Wxcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '微信openid',
  `HeadUrl` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '头像地址',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 26 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_user
-- ----------------------------
INSERT INTO `pub_user` VALUES (1, '00000001', 'chi1', '迟', '123123', 0, NULL, NULL, 'D000001', 0, '152881331161', NULL, NULL, NULL, 0, NULL, NULL, '00000002-admin', '2022-04-21 14:22:24', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (2, '00000002', 'admin', 'admin', '123456', 0, NULL, NULL, 'D000002', 0, '15288133116', NULL, NULL, '                        \n                    ', 0, NULL, NULL, '00000002-admin', '2020-08-04 09:45:34', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (19, '00000003', 'zsx', 'chi', '123456', 1, NULL, NULL, 'D000001', 0, '1', NULL, NULL, NULL, 1, '00000002-admin', '2018-08-17 10:18:58', '00000002-admin', '2018-08-17 10:18:58', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (20, '00000004', 'cts', '迟1', '123123', 1, NULL, NULL, 'D000003', 0, '12345678901', NULL, NULL, '123456', 0, '00000002-admin', '2019-08-01 17:29:09', '00000002-admin', '2019-10-10 14:17:38', NULL, NULL, NULL, NULL, '123', NULL, NULL);
INSERT INTO `pub_user` VALUES (21, '00000005', 'cts2', '111', '1234567890', 1, NULL, NULL, 'D000047', 0, '123123457890', NULL, NULL, '2222', 1, '00000002-admin', '2019-08-01 17:33:36', '00000002-admin', '2019-08-02 15:35:19', NULL, NULL, NULL, NULL, '111', NULL, NULL);
INSERT INTO `pub_user` VALUES (22, '00000006', 'jack', 'chi', '123456', 1, NULL, NULL, 'D000003', 0, '15288133116', NULL, NULL, NULL, 1, '00000002-admin', '2019-08-02 08:56:02', '00000002-admin', '2019-08-02 09:00:07', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (23, '00000007', '123', NULL, '11111111', 1, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, '00000002-admin', '2019-08-02 16:52:01', '00000002-admin', '2019-08-02 16:52:01', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (24, '00000008', '123', NULL, '11111111', 1, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, '00000002-admin', '2019-08-02 16:52:20', '00000002-admin', '2019-08-02 16:52:20', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (25, '00000009', 'chi', '111', '123123', 1, NULL, NULL, 'D000003', 0, '11', NULL, NULL, NULL, 1, '00000001-chi', '2019-10-12 14:08:31', '00000001-chi', '2019-10-12 14:08:31', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `pub_user` VALUES (26, '00000010', 'tt1', NULL, '123123', 1, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, '00000002-admin', '2022-04-21 15:09:12', '00000002-admin', '2022-04-21 15:09:12', NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for pub_userfunction
-- ----------------------------
DROP TABLE IF EXISTS `pub_userfunction`;
CREATE TABLE `pub_userfunction`  (
  `Id` int(0) NOT NULL,
  `UserCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户编号',
  `FunctionCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '权限编号',
  `Lmid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '最后编辑人',
  `Lmdt` datetime(0) NULL DEFAULT NULL COMMENT '最后编辑时间',
  `StopFlag` tinyint(0) NULL DEFAULT NULL COMMENT '停用状态 默认0 未停用 1 停用',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_userfunction
-- ----------------------------
INSERT INTO `pub_userfunction` VALUES (11, '00000003', 'FC001', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (12, '00000003', 'FC001002', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (17, '00000005', '', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (137, '00000002', 'FC001004001', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (138, '00000002', 'FC001004002', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (139, '00000002', 'FC001004003', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (140, '00000002', 'FC001004004', NULL, NULL, NULL);
INSERT INTO `pub_userfunction` VALUES (141, '00000002', 'FC001005', NULL, NULL, NULL);

-- ----------------------------
-- Table structure for pub_userrole
-- ----------------------------
DROP TABLE IF EXISTS `pub_userrole`;
CREATE TABLE `pub_userrole`  (
  `Id` int(0) NOT NULL COMMENT '自增主键',
  `UserCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户编号',
  `RoleCode` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色编号',
  `Lmid` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '最后编辑人',
  `Lmdt` datetime(0) NULL DEFAULT NULL COMMENT '最后编辑时间',
  `StopFlag` tinyint(0) NULL DEFAULT NULL COMMENT '停用状态 默认0 未停用 1 停用',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pub_userrole
-- ----------------------------
INSERT INTO `pub_userrole` VALUES (11, '00000003', 'RC000001', NULL, NULL, NULL);
INSERT INTO `pub_userrole` VALUES (12, '00000003', 'RC000002', NULL, NULL, NULL);
INSERT INTO `pub_userrole` VALUES (29, '00000006', 'RC000002', NULL, NULL, NULL);
INSERT INTO `pub_userrole` VALUES (31, '00000005', 'RC000002', NULL, NULL, NULL);
INSERT INTO `pub_userrole` VALUES (40, '00000002', 'RC000001', NULL, NULL, NULL);
INSERT INTO `pub_userrole` VALUES (42, '00000004', 'RC000003', NULL, NULL, NULL);
INSERT INTO `pub_userrole` VALUES (44, '00000009', 'RC000001', NULL, NULL, NULL);

-- ----------------------------
-- View structure for v_pubdept_parent
-- ----------------------------
DROP VIEW IF EXISTS `v_pubdept_parent`;
CREATE ALGORITHM = UNDEFINED SQL SECURITY DEFINER VIEW `v_pubdept_parent` AS select `pub_department`.`DeptCode` AS `DeptCode`,`pub_department`.`DeptName` AS `DeptName`,`pub_department`.`Remark` AS `Remark`,`pub_department`.`ParentCode` AS `ParentCode`,`pub_department`.`DeptLevel` AS `DeptLevel`,`pub_department`.`Lmid` AS `Lmid`,`pub_department`.`Lmdt` AS `Lmdt`,`pub_department`.`StopFlag` AS `StopFlag`,`pub_department_1`.`DeptName` AS `ParentName` from (`pub_department` join `pub_department` `pub_department_1` on((`pub_department`.`ParentCode` = `pub_department_1`.`DeptCode`)));

-- ----------------------------
-- View structure for v_pubfunction_parent
-- ----------------------------
DROP VIEW IF EXISTS `v_pubfunction_parent`;
CREATE ALGORITHM = UNDEFINED SQL SECURITY DEFINER VIEW `v_pubfunction_parent` AS select `pub_function`.`FunctionCode` AS `FunctionCode`,`pub_function`.`FunctionEnglish` AS `FunctionEnglish`,`pub_function`.`FunctionChina` AS `FunctionChina`,`pub_function`.`FunctionDescrip` AS `FunctionDescrip`,`pub_function`.`ParentCode` AS `ParentCode`,`pub_function`.`MenuFlag` AS `MenuFlag`,`pub_function`.`StopFlag` AS `StopFlag`,`pub_function`.`URLString` AS `URLString`,`pub_function`.`editdate` AS `editdate`,`pub_function`.`editor` AS `editor`,`pub_function`.`sortidx` AS `sortidx`,`pub_function`.`target` AS `target`,`pub_function`.`MenuIcon` AS `MenuIcon`,`pub_function_1`.`FunctionChina` AS `parentName`,`pub_function`.`RouterPath` AS `RouterPath`,`pub_function`.`IsCache` AS `IsCache` from (`pub_function` left join `pub_function` `pub_function_1` on((`pub_function`.`ParentCode` = `pub_function_1`.`FunctionCode`)));

-- ----------------------------
-- View structure for v_pubuser_dept
-- ----------------------------
DROP VIEW IF EXISTS `v_pubuser_dept`;
CREATE ALGORITHM = UNDEFINED SQL SECURITY DEFINER VIEW `v_pubuser_dept` AS select `pub_user`.`Id` AS `Id`,`pub_user`.`UserCode` AS `UserCode`,`pub_user`.`UserName` AS `UserName`,`pub_user`.`RealName` AS `RealName`,`pub_user`.`UserPwd` AS `UserPwd`,`pub_user`.`Sex` AS `Sex`,`pub_user`.`IdentityNo` AS `IdentityNo`,`pub_user`.`Birthday` AS `Birthday`,`pub_user`.`DeptCode` AS `DeptCode`,`pub_user`.`ManagerFlag` AS `ManagerFlag`,`pub_user`.`Tel` AS `Tel`,`pub_user`.`EMail` AS `EMail`,`pub_user`.`QQ` AS `QQ`,`pub_user`.`Remark` AS `Remark`,`pub_user`.`StopFlag` AS `StopFlag`,`pub_user`.`Crid` AS `Crid`,`pub_user`.`Crdt` AS `Crdt`,`pub_user`.`Lmid` AS `Lmid`,`pub_user`.`Lmdt` AS `Lmdt`,`pub_user`.`LoginDate` AS `LoginDate`,`pub_user`.`ProvinceCode` AS `ProvinceCode`,`pub_user`.`CityCode` AS `CityCode`,`pub_user`.`RegionCode` AS `RegionCode`,`pub_user`.`UserAddress` AS `UserAddress`,`pub_user`.`Wxcode` AS `Wxcode`,`pub_user`.`HeadUrl` AS `HeadUrl`,`pub_department`.`DeptName` AS `DeptName` from (`pub_user` left join `pub_department` on((`pub_department`.`DeptCode` = `pub_user`.`DeptCode`)));

-- ----------------------------
-- Procedure structure for pr_pager
-- ----------------------------
DROP PROCEDURE IF EXISTS `pr_pager`;
delimiter ;;
CREATE PROCEDURE `pr_pager`(in     p_table_name        varchar(1024),        /*表名*/

in     p_fields            varchar(1024),        /*查询字段*/

in     p_page_size            int,                /*每页记录数*/

in     p_page_now            int,                /*当前页*/

in     p_order_string        varchar(128),        /*排序条件(包含order关键字,可为空)*/

in     p_where_string        varchar(1024),        /*where条件(包含where关键字,可为空)*/

out    p_out_rows            int)
  COMMENT '分页存储过程'
begin

/*定义变量*/

declare m_begin_row int default 0;

declare m_limit_string char(64);

/*构造语句*/

set m_begin_row = (p_page_now - 1) * p_page_size;

set m_limit_string = concat(' limit ', m_begin_row, ', ', p_page_size);

set @count_string = concat('select count(*) into @rows_total from ', p_table_name, ' where  ', p_where_string);

set @main_string = concat('select ', p_fields, ' from ', p_table_name, ' where ', p_where_string, ' order by ', p_order_string, m_limit_string);

/*预处理*/

prepare count_stmt from @count_string;

execute count_stmt;

deallocate prepare count_stmt;

set p_out_rows = @rows_total;

prepare main_stmt from @main_string;

execute main_stmt;

deallocate prepare main_stmt;

end
;;
delimiter ;

-- ----------------------------
-- Procedure structure for P_GetMenu
-- ----------------------------
DROP PROCEDURE IF EXISTS `P_GetMenu`;
delimiter ;;
CREATE PROCEDURE `P_GetMenu`(in userCode VARCHAR(16))
BEGIN
	select DISTINCT * from (

		SELECT pf.* FROM Pub_Function AS pf
		WHERE pf.StopFlag=0 AND  EXISTS(SELECT prf.Id FROM  Pub_RoleFunction prf WHERE pf.FunctionCode= prf.FunctionCode AND
		 prf.RoleCode IN(SELECT pur.RoleCode FROM Pub_UserRole AS pur WHERE pur.UserCode=userCode ) )
		UNION 
		SELECT pf.* FROM Pub_Function AS pf
		WHERE pf.StopFlag=0 AND EXISTS(SELECT puf.Id FROM Pub_UserFunction AS puf WHERE pf.FunctionCode=puf.FunctionCode AND puf.UserCode=userCode)
	  ) t where t.MenuFlag=1;

END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for P_GetUserAccess
-- ----------------------------
DROP PROCEDURE IF EXISTS `P_GetUserAccess`;
delimiter ;;
CREATE PROCEDURE `P_GetUserAccess`(in userCode VARCHAR(16))
BEGIN
	select DISTINCT FunctionCode from (

		SELECT pf.* FROM Pub_Function AS pf
		WHERE pf.StopFlag=0 AND  EXISTS(SELECT prf.Id FROM  Pub_RoleFunction prf WHERE pf.FunctionCode= prf.FunctionCode AND
		 prf.RoleCode IN(SELECT pur.RoleCode FROM Pub_UserRole AS pur WHERE pur.UserCode=userCode ) )
		UNION 
		SELECT pf.* FROM Pub_Function AS pf
		WHERE pf.StopFlag=0 AND EXISTS(SELECT puf.Id FROM Pub_UserFunction AS puf WHERE pf.FunctionCode=puf.FunctionCode AND puf.UserCode=userCode)
	  ) t where t.StopFlag=0;

END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for p_SearchChildDept
-- ----------------------------
DROP PROCEDURE IF EXISTS `p_SearchChildDept`;
delimiter ;;
CREATE PROCEDURE `p_SearchChildDept`(in deptCodeIn VARCHAR(20))
BEGIN
select *,(select DeptName from pub_department where deptCode=a.ParentCode LIMIT 1) as ParentName FROM
(
select * from pub_department where DeptCode = deptCodeIn
UNION
SELECT au.*
FROM (SELECT * FROM pub_department WHERE IFNULL(ParentCode,'')<>'') au,
     (SELECT @pid := deptCodeIn) pd 
WHERE FIND_IN_SET(ParentCode, @pid) > 0 
  AND  (IFNULL(@pid := concat(@pid, ',', DeptCode),'')<>'' )
) a;

END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for p_SearchChildFunction
-- ----------------------------
DROP PROCEDURE IF EXISTS `p_SearchChildFunction`;
delimiter ;;
CREATE PROCEDURE `p_SearchChildFunction`(in functionCodeIn VARCHAR(20))
BEGIN
select *,(select FunctionChina from pub_function where FunctionCode=a.ParentCode LIMIT 1) as ParentName FROM
(
select * from pub_function where FunctionCode = functionCodeIn
UNION
SELECT au.*
FROM (SELECT * FROM pub_function WHERE IFNULL(ParentCode,'')<>'') au,
     (SELECT @pid := functionCodeIn) pd 
WHERE FIND_IN_SET(ParentCode, @pid) > 0 
  AND  (IFNULL(@pid := concat(@pid, ',', FunctionCode),'')<>'' )
) a;

END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
