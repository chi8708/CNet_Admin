<style scoped>
.ivu-layout {
    height: 100%;
}

.ivu-menu-vertical {
    height: calc(100% - 70px);
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

/*菜单树样式*/
.slider-menu {
    overflow-y: auto;
    padding-left: 15px;
    padding-top: 15px;
    border-right: 1px solid #dcdee2;
    height: calc(100% - 60px);
}
</style>

<template>
    <div class="layout">
        <Layout>
            <Sider ref="side1" :style="{ background: '#fff' }" hide-trigger collapsible :collapsed-width="78"
                v-model="isCollapsed" width="260">
                <h2 style="padding:15px; padding-bottom: 17px; border: 1px solid #dcdee2;border-top: 0px;">知识库管理系统
                    <Icon type="ios-help-circle-outline" />
                </h2>
                <!-- <Menu active-name="1-2" theme="light" width="auto" :class="menuitemClasses" class="slider-menu">
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
                </Menu> -->
                <div :class="menuitemClasses" class="slider-menu">
                    <sort-tree ref="sortTree" :parent="this" :isManage="false"></sort-tree>
                </div>

            </Sider>
            <Layout>
                <Header :style="{ padding: 0 }" class="layout-header-bar">

                    <Row align="middle">
                        <Col span="1">
                        <Icon @click="collapsedSider" :class="rotateIcon" :style="{ margin: '0 20px' }" type="md-menu"
                            size="24">
                        </Icon>
                        </Col>
                        <!-- <Col span="3">
                            <h2>知识库管理系统<Icon type="ios-help-circle-outline" /></h2>
                        </Col> -->
                        <Col span="12">

                        <Input v-model="queryData.S_Keyword" search placeholder="输入关键字搜索..." enter-button="搜索"
                            @on-search="queryPage" />
                        </Col>
                    </Row>

                </Header>
                <strong style="margin: 20px;">搜索结果:</strong>
                <Content :style="{ margin: '20px', background: '#fff', minHeight: '260px', marginTop: '0px' }">
                    <Table max-height="700" ref="tables" :data="tableData1" v-bind:columns="tableColumns1" stripe>
                        <template slot-scope="{ row, index }" slot="action">
                            <Button type="error" size="small" icon="md-trash" @click="handleDownload(row)">下载</Button>
                        </template>
                    </Table>
                    <div style="margin: 10px;overflow: hidden">
                        <div style="float: right;">
                            <Page :total="pageTotal" :current="pageCurrent" @on-change="changePage"
                                @on-page-size-change="changePageSize" show-total show-elevator show-sizer></Page>
                        </div>
                    </div>
                </Content>
            </Layout>
        </Layout>
    </div>
</template>
<script>
import SortTree from "../WikiManage/SortTree";
import { getPage } from "@/api/wikiMain"
import config from "@/config";
export default {
    components: {
        SortTree
    },
    data() {
        return {
            isCollapsed: false,
            tableData1: [],
            pageTotal: 0,
            pageCurrent: 1,
            queryData: { S_SortCode: '', S_Keyword: '' },
            tableColumns1: [
                {
                    title: "标题",
                    key: "title",
                    render: (h, params) => {
                        return h('span',
                            {
                                style: {
                                    cursor: 'pointer',
                                    display: 'inline-block',
                                    width: '100%',
                                },
                                on: {
                                    click: () => {this.readInfo(params.row)}
                                }
                            },
                            [
                                h('Icon', {
                                    props: {
                                        type: 'md-document'
                                    },
                                    style: {
                                        marginRight: '8px',
                                        
                                    }
                                }),
                                h('strong', params.row.title)
                            ])
                    }
                },
                {
                    title: "类别",
                    key: "sortName",
                    width: 160
                },
                {
                    title: "更新时间",
                    key: "editTime",
                    width: 180
                },
                {
                    title: '操作',
                    slot: 'action',
                    width: 140,
                    align: 'center'
                }
            ]
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
        setPageData(pageNum = this.pageCurrent, pageSize = 10) {
            getPage({
                pageNum: pageNum,
                pageSize: pageSize,
                field: "Id",
                order: "asc",
                query: this.queryData
            })
                .then(res => {
                    const resData = res.data;
                    const code = resData.code;
                    const msg = resData.msg;
                    if (code != 1) {
                        this.$Message.error(code.msg);
                        return;
                    }
                    const data = resData.data;
                    this.tableData1 = data;
                    this.pageTotal = resData.count;
                    this.pageCurrent = resData.PageNum;
                })
                .catch(err => { });
        },
        queryPage() {
            this.queryData.S_SortCode = '';
            this.setPageData(1);
        },
        changePage(page) {
            this.setPageData(page);
        },
        changePageSize(pageSize) {
            this.setPageData(1, pageSize);
        },
        sortChange(code) {
            this.queryData.S_SortCode = code;
            this.setPageData(1);
        },
        handleDownload(row) {

        },
        //读取信息
        readInfo(row){
          const fullPath=config.imgurl+row.filePath;
          window.open(fullPath, '_blank'); 
        },
        collapsedSider() {
            this.$refs.side1.toggleCollapse();
        }
    },
    mounted() {
        this.setPageData();
    }
}
</script>
