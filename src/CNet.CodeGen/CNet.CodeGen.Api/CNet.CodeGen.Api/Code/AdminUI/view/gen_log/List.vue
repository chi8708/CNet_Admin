

        <!-- 此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-03-30 20:48:14 -->
    
<template>
  <div class="content-main">
    <div class="search-con search-con-top">
      <Form ref="formInline" label-position="right" :label-width="60" inline>
                  <FormItem label="创建时间">
                            <Input class="search-input" v-model="queryData.SL_CreateTime" />
                  </FormItem>
                  <FormItem label="">
                            <Input class="search-input" v-model="queryData.SL_GenInfo" />
                  </FormItem>
                  <FormItem label="">
                            <Input class="search-input" v-model="queryData.SL_Id" />
                  </FormItem>
                  <FormItem label="">
                            <Input class="search-input" v-model="queryData.SL_Status" />
                  </FormItem>
          <Button class="search-btn" type="primary" @click="setPageData(1)">
            <Icon type="search" />&nbsp;&nbsp;搜索
          </Button>
      </Form>
    </div>

    <Card>
      <div>
        <Button v-if="userAccess.isAdd" class="search-btn" type="success" size="small" @click="handleAdd">
          <Icon type="md-add" />&nbsp;&nbsp;新增
        </Button>
      </div>
      <Table ref="tables" :data="tableData1" v-bind:columns="tableColumns1" stripe>
        <template slot-scope="{ row, index }" slot="action">
            <Button v-if="userAccess.isEdit" type="primary" size="small" icon="md-create" style="margin-right: 5px" @click="handleEdit(row)">编辑</Button>
            <Button v-if="userAccess.isMove" type="error"  size="small" icon="md-trash" @click="handleDelete(row)">删除</Button>
        </template>
      </Table>
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page
            :total="pageTotal"
            :current="pageCurrent"
            @on-change="changePage"
            @on-page-size-change="changePageSize"
            show-total
            show-elevator
            show-sizer
          ></Page>
        </div>
      </div>
    </Card>

    <!--垂直居中 class-name="vertical-center-modal" 和draggable冲突 draggable后mask强制变为false-->
    <Modal
      title="编辑"
      :mask-closable="false"
      v-model="modelEdit"
      width="800"
      scrollable
      footer-hide
      @on-ok="saveEdit"
    >
      <Edit ref="edit" :parent="this" :edit-row="eidtRow"></Edit>
    </Modal>
  </div>
</template>
<script>
import Edit from "./Edit";
import { getPage, remove } from "@/api/gen_log";
import { gen_log } from "@/access/gen_log"
export default {
  name: 'gen_log',//与 router.js notCache:fasle且name相同 将缓存组件。
  components: {
    Edit,
  },
  data() {
    const userAccessAll=this.$store.state.user.access;
    return {
      userAccess:{
        isAdd: userAccessAll.includes(`${gen_log.ADD}`),
        isEdit: userAccessAll.includes(`${gen_log.EDIT}`),
        isMove: userAccessAll.includes(`${gen_log.EDIT}`),
      },
      tableData1: [],
      queryData: {},
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      isAdd: true,
      eidtRow: {},
      tableColumns1: [

            {
                title:"创建时间",
                key: "createTime"
             },
           
            {
                title:"",
                key: "genInfo"
             },
           
            {
                title:"",
                key: "id"
             },
           
            {
                title:"",
                key: "status"
             },
                   {
         title: '操作',
         slot: 'action',
         width: 300,
         align: 'center'
        }
        
      ]
    };
  },
  computed: {},
  methods: {
    setPageData(pageNum = this.pageCurrent, pageSize = 10) {
      getPage({
        pageNum: pageNum,
        pageSize: pageSize,
        field: "Id",
        order: "desc",
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
        .catch(err => {});
    },
    changePage(page) {
      this.setPageData(page);
    },
    changePageSize(pageSize) {
      this.setPageData(1, pageSize);
    },
    handleDelete(row) {
      this.$Modal.confirm({
        title: "提示",
        content: "<p>确定要删除[" + row.Id+ "]?</p>",
        onOk: () => {
          this.remove(row);
        },
        onCancel: row => {}
      });
    },
    handleAdd() {
      this.$refs.edit.handleReset();
      this.modelEdit = true;
      this.isAdd = true;
      this.eidtRow = {};
    },
    handleEdit(row) {
      this.modelEdit = true;
      this.isAdd = false;
      this.eidtRow = row;
    },
    remove(row) {
      var id = row.Id;
      remove(id)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("删除成功");
            this.setPageData();
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true });
          }
        })
        .catch(err => {});
    },
    saveEdit() {
      var row = this.$refs.edit.Row;
    }
  },
  mounted() {
    this.setPageData(1);
  }
};
</script>

