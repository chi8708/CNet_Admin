
<template>
  <div class="content-main">
    <div class="search-con search-con-top">
      <Form ref="formInline" label-position="right" :label-width="60" inline>
        <FormItem label="用户名">
          <Input class="search-input" v-model="queryData.SL_UserName" />
        </FormItem>
        <FormItem label="电话">
          <Input class="search-input" v-model="queryData.SL_Tel" />
        </FormItem>
        <FormItem>
          <Button class="search-btn" type="primary" @click="setPageData(1)">
            <Icon type="search" />&nbsp;&nbsp;搜索
          </Button>
        </FormItem>
      </Form>
    </div>

    <Card>
      <div>
        <Button v-if="userAccess.isAdd" class="search-btn" type="success"  size="small" @click="handleAdd">
          <Icon type="md-add" />&nbsp;&nbsp;新增
        </Button>
      </div>
      <!-- <Table ref="tables" :data="tableData1" v-bind:columns="tableColumns1" stripe>
      </Table> -->
     <Table ref="tables" border :data="tableData1" v-bind:columns="tableColumns1" stripe>
        <template slot-scope="{ row, index }" slot="action">
            <Button v-if="userAccess.isAuth" type="default"  size="small" icon="md-key" style="margin-right: 5px" @click="handlePermission(row)">授权</Button>
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
      <Edit ref="edit" v-if="modelEdit" :parent="this" :edit-row="eidtRow"></Edit>
    </Modal>

     <Modal
      title="授权"
      :mask-closable="false"
      v-model="modelPermission"
      width="300"
      scrollable
      footer-hide
    >
      <Permission ref="Permission" :parent="this" :edit-row="eidtRow"></Permission>
    </Modal>

  </div>
</template>
<script>
//import Tables from '_c/tables'
//import "@/assets/css/util.less";
import Edit from "./Edit";
import Permission from "./Permission";
import { getPage, remove as removeUser } from "@/api/pubUser";
import { pubUser } from "@/access/pubUser"
import { log } from 'util';

export default {
  name: 'USERINFO',//与 router.js notCache:fasle且name相同 将缓存组件。
  components: {
    // Tables
    Edit,
    Permission
  },
  data() {
    const userAccessAll=this.$store.state.user.access;
    return {
      userAccess:{
        isAdd:userAccessAll.includes(`${pubUser.ADD}`),
        isEdit:userAccessAll.includes(`${pubUser.EDIT}`),
        isMove:userAccessAll.includes(`${pubUser.REMOVE}`),
        isAuth:userAccessAll.includes(`${pubUser.AUTH}`)
      },
      tableData1: [],
      queryData: {},
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      modelPermission:false,
      isAdd: true,
      eidtRow: {},
      tableColumns1: [
        {
          title: "编号",
          key: "id"
        },
        {
          title: "用户名",
          key: "userName"
        },
        {
          title: "真实姓名",
          key: "realName"
        },
        {
          title: "电话",
          key: "tel"
        },
        {
          title: "部门",
          key: "deptName"
        },
        {
          title: "修改时间",
          key: "lmdt"
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
        .catch(err => {});
    },
    formatDate(date) {
      const y = date.getFullYear();
      let m = date.getMonth() + 1;
      m = m < 10 ? "0" + m : m;
      let d = date.getDate();
      d = d < 10 ? "0" + d : d;
      return y + "-" + m + "-" + d;
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
        content: "<p>确定要删除[" + row.id + "]?</p>",
        onOk: () => {
          // this.$Message.info("Clicked ok");
          this.removeUser(row);
        },
        onCancel: row => {}
      });
    },
    handleAdd() {
    //  this.$refs.edit.handleReset();
      this.modelEdit = true;
      this.isAdd = true;
      this.eidtRow = {};
    },
    handleEdit(row) {
      this.modelEdit = true;
      this.isAdd = false;
      this.eidtRow = row;
    },
    handlePermission(row) {
      this.modelPermission = true;
      this.eidtRow = row;
    },
    removeUser(row) {
      var id = row.id;
      removeUser(id)
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

