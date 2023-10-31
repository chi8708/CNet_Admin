<style scoped>
.ivu-layout {
    height: 100%;
}

.ivu-menu-vertical {
    height: 100%;
}

.layout {
    border: 1px solid #d7dde4;
    background: #f5f7f9;
    position: relative;
    border-radius: 4px;
    overflow: hidden;
    height: 100%;
}

.layout .ivu-menu {
    z-index: 0
}

.layout-header-bar {
    background: #fff;
    box-shadow: 0 1px 1px rgba(0, 0, 0, .1);
}

.layout-logo-left {
    width: 90%;
    height: 30px;
    background: #5b6270;
    border-radius: 3px;
    margin: 15px auto;
}

.menu-icon {
    transition: all .3s;
}

.rotate-icon {
    transform: rotate(-90deg);
}

.menu-item span {
    display: inline-block;
    overflow: hidden;
    width: 69px;
    text-overflow: ellipsis;
    white-space: nowrap;
    vertical-align: bottom;
    transition: width .2s ease .2s;
}

.menu-item i {
    transform: translateX(0px);
    transition: font-size .2s ease, transform .2s ease;
    vertical-align: middle;
    font-size: 16px;
}

.collapsed-menu span {
    width: 0px;
    transition: width .2s ease;
}

.collapsed-menu ul {
    width: 0px;
}

/*收缩后菜单样式*/
::v-deep .collapsed-menu .ivu-icon-ios-arrow-down,
::v-deep .collapsed-menu ul {
    display: none;
}
.collapsed-menu i {
    transform: translateX(5px);
    transition: font-size .2s ease .2s, transform .2s ease .2s;
    vertical-align: middle;
    font-size: 22px;
}
/*菜单滚动条样式*/
.ivu-layout-sider{
    overflow: auto;
}
</style>
<template>
    <div class="layout">
        <Layout>
            <Sider ref="side1" :style="{ background: '#fff' }" hide-trigger collapsible :collapsed-width="78"
                v-model="isCollapsed">
                <Menu active-name="1-2" theme="light" width="auto" :class="menuitemClasses">
                    <Submenu name="1" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-navigate"></Icon>
                            <span>Item 1</span>
                        </template>
                        <Submenu name="4_1">
                            <template #title>Submenu</template>
                            <MenuItem name="4-1">Option 7</MenuItem>
                            <MenuItem name="4-2">Option 8</MenuItem>
                        </Submenu>
                        <MenuItem name="1-1">Option 1</MenuItem>
                        <MenuItem name="1-2">Option 2</MenuItem>
                        <MenuItem name="1-3">Option 3</MenuItem>
                    </Submenu>
                    <Submenu name="2" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-keypad"></Icon>
                            <span>Item 2</span>
                        </template>
                        <MenuItem name="2-1">Option 1</MenuItem>
                        <MenuItem name="2-2">Option 2</MenuItem>
                    </Submenu>
                    <Submenu name="3" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-analytics"></Icon>
                            <span>Item 3</span>
                        </template>
                        <MenuItem name="3-1">Option 1</MenuItem>
                        <MenuItem name="3-2">Option 2</MenuItem>
                    </Submenu>
                    <Submenu name="3" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-analytics"></Icon>
                            <span>Item 3</span>
                        </template>
                        <MenuItem name="3-1">Option 1</MenuItem>
                        <MenuItem name="3-2">Option 2</MenuItem>
                    </Submenu>
                    <Submenu name="3" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-analytics"></Icon>
                            <span>Item 3</span>
                        </template>
                        <MenuItem name="3-1">Option 1</MenuItem>
                        <MenuItem name="3-2">Option 2</MenuItem>
                    </Submenu>
                    <Submenu name="3" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-analytics"></Icon>
                            <span>Item 3</span>
                        </template>
                        <MenuItem name="3-1">Option 1</MenuItem>
                        <MenuItem name="3-2">Option 2</MenuItem>
                    </Submenu>
                    <Submenu name="3" :class="menuitemClasses">
                        <template #title>
                            <Icon type="ios-analytics"></Icon>
                            <span>Item 3</span>
                        </template>
                        <MenuItem name="3-1">Option 1</MenuItem>
                        <MenuItem name="3-2">Option 2</MenuItem>
                    </Submenu>
                </Menu>
            </Sider>
            <Layout>
                <Header :style="{ padding: 0 }" class="layout-header-bar">

                    <Row align="middle">
                        <Col span="1">
                        <Icon @click="collapsedSider" :class="rotateIcon" :style="{ margin: '0 20px' }" type="md-menu"
                            size="24">
                        </Icon>
                        </Col>
                        <Col span="3">
                            <h2>知识库管理系统<Icon type="ios-help-circle-outline" /></h2>
                        </Col>
                        <Col span="12"> <Input search enter-button placeholder="输入关键字搜索..." /></Col>
                    </Row>

                </Header>
                <strong style="margin: 20px;">搜索结果:</strong>
                <Content :style="{ margin: '20px', background: '#fff', minHeight: '260px', marginTop: '0px' }">
                    <List>
                        <ListItem>
                            <a href="http://www.baidu.com" target="_blank">test</a>
                        </ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>

                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                        <ListItem>This is a piece of text.</ListItem>
                    </List>
                </Content>
            </Layout>
        </Layout>
    </div>
</template>
<script>
export default {
    data() {
        return {
            isCollapsed: false
        }
    },
    computed: {
        rotateIcon() {
            return [
                'menu-icon',
                this.isCollapsed ? 'rotate-icon' : ''
            ];
        },
        menuitemClasses() {
            return [
                'menu-item',
                this.isCollapsed ? 'collapsed-menu' : ''
            ]
        },
        menuitemClasses_Main() {
            return [
                this.isCollapsed ? 'collapsed-menu_Main' : ''
            ]
        }
    },
    methods: {
        collapsedSider() {
            this.$refs.side1.toggleCollapse();
        }
    }
}
</script>
