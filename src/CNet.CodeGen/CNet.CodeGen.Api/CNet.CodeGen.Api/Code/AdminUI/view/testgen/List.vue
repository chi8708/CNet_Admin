

        <!-- 此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-03-30 23:43:16 -->
    
<template>
  <div class="content-main">
    <div class="search-con search-con-top">
      <Form ref="formInline" label-position="right" :label-width="60" inline>
                  <FormItem label="创建时间">
                            <Input class="search-input" v-model="queryData.SL_Crdt" />
                  </FormItem>
                  <FormItem label="创建人">
                            <Input class="search-input" v-model="queryData.SL_Crid" />
                  </FormItem>
                  <FormItem label="">
                            <Input class="search-input" v-model="queryData.SL_Id" />
                  </FormItem>
                  <FormItem label="最后更新时间">
                            <Input class="search-input" v-model="queryData.SL_Lmdt" />
                  </FormItem>
                  <FormItem label="最后更新人">
                            <Input class="search-input" v-model="queryData.SL_Lmid" />
                  </FormItem>
                  <FormItem label="备注">
                            <Input class="search-input" v-model="queryData.SL_Remark" />
                  </FormItem>
                  <FormItem label="角色编号">
                            <Input class="search-input" v-model="queryData.SL_RoleCode" />
                  </FormItem>
                  <FormItem label="角色名称">
                            <Input class="search-input" v-model="queryData.SL_RoleName" />
                  </FormItem>
                  <FormItem label="停用状态 默认0  未停用 1 停用">
                            <Input class="search-input" v-model="queryData.SL_StopFlag" />
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
import { getPage, remove } from "@/api/testgen";
import { testgen } from "@/access/testgen"
export default {
  name: 'testgen',//与 router.js notCache:fasle且name相同 将缓存组件。
  components: {
    Edit,
  },
  data() {
    const userAccessAll=this.$store.state.user.access;
    return {
      userAccess:{
        isAdd: userAccessAll.includes(`${testgen.ADD}`),
        isEdit: userAccessAll.includes(`${testgen.EDIT}`),
        isMove: userAccessAll.includes(`${testgen.EDIT}`),
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
                key: "crdt"
             },
           
            {
                title:"创建人",
                key: "crid"
             },
           
            {
                title:"",
                key: "id"
             },
           
            {
                title:"最后更新时间",
                key: "lmdt"
             },
           
            {
                title:"最后更新人",
                key: "lmid"
             },
           
            {
                title:"备注",
                key: "remark"
             },
           
            {
                title:"角色编号",
                key: "roleCode"
             },
           
            {
                title:"角色名称",
                key: "roleName"
             },
           
            {
                title:"停用状态 默认0  未停用 1 停用",
                key: "stopFlag"
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

