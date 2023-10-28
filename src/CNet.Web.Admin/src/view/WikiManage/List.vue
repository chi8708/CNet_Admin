<style>
.content-main {
  height: 99%;
  border: 1px solid #dcdee2;
  /* background-color: #fff; */
}

.demo-split-pane {
  padding: 10px;
}
</style>
<template>
  <div class="content-main">
    <Split v-model="split1" max="300" min="300" style="max-height:100%;overflow:hidden;">
      <div slot="left" class="demo-split-pane" style="max-height:100%;overflow:auto;">
        <h3>分类</h3>
        <div style="max-height:97%;overflow:auto;">
          <sort-tree ref="sortTree" :parent="this"></sort-tree>
        </div>
      </div>
      <div slot="right" class="demo-split-pane" style="max-height:100%;overflow:auto;">
        <h3>知识库列表</h3>
        <div>
          <Button v-if="userAccess.isAdd" class="search-btn" type="success" size="small" @click="handleAdd">
            <Icon type="md-add" />&nbsp;&nbsp;新增
          </Button>
          <Table max-height ="700"  ref="tables" border :data="tableData1" v-bind:columns="tableColumns1" stripe>
            <template slot-scope="{ row, index }" slot="action">

              <Button v-if="userAccess.isEdit" type="primary" size="small" icon="md-create" style="margin-right: 5px"
                @click="handleEdit(row)">编辑</Button>
              <Button v-if="userAccess.isMove" type="error" size="small" icon="md-trash"
                @click="handleDelete(row)">删除</Button>
            </template>
          </Table>
          <div style="margin: 10px;overflow: hidden">
            <div style="float: right;">
              <Page :total="pageTotal" :current="pageCurrent" @on-change="changePage"
                @on-page-size-change="changePageSize" show-total show-elevator show-sizer></Page>
            </div>
          </div>
        </div>


      </div>
    </Split>
    <!--垂直居中 class-name="vertical-center-modal" 和draggable冲突 draggable后mask强制变为false-->
    <Modal title="编辑" :mask-closable="false" v-model="modelEdit" width="800" scrollable footer-hide @on-ok="saveEdit">
      <Edit v-if="modelEdit" ref="edit" :parent="this" :edit-row="eidtRow"></Edit>
    </Modal>
  </div>
</template>
<script>
import SortTree from "./SortTree"
import Edit from "./Edit"
import { remove } from "@/api/wikiSort"
import { getPage } from "@/api/wikiMain"
import { wiki } from "@/access/wikiMain"

export default {
  data() {
    const userAccessAll = this.$store.state.user.access;
    return {
      userAccess: {
        isAdd: userAccessAll.includes(`${wiki.ADD}`),
        isEdit: userAccessAll.includes(`${wiki.EDIT}`),
        isMove: userAccessAll.includes(`${wiki.REMOVE}`),
        isAuth: userAccessAll.includes(`${wiki.SORT}`)
      },
      split1: "300",
      tableData1: [],
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      isAdd: true,
      eidtRow: {},
      selectedCode: "",
      tableColumns1: [
        {
          title: "编号",
          key: "sortCode"
        },
        {
          title: "名称",
          key: "sortName"
        },
        {
          title: "修改时间",
          key: "lmdt"
        },
        {
          title: "修改人",
          key: "lmid"
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
  components: {
    SortTree,
    Edit
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
    changePage(page) {
      this.setPageData(page);
    },
    changePageSize(pageSize) {
      this.setPageData(1, pageSize);
    },
    handleDelete(row) {
      this.$Modal.confirm({
        title: "提示",
        content: "<p>确定要删除[" + row.sortCode + "]?</p>",
        onOk: () => {
          this.remove(row);
        },
        onCancel: row => { }
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
      var id = row.sortCode;
      remove(id)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("删除成功");
            this.reloadAll(this.selectedCode);
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true });
          }
        })
        .catch(err => { });
    },
    saveEdit() {
      var row = this.$refs.edit.Row;
    },
    sortChange(code) {
      this.selectedCode = code;
      this.setPageData();
    },
    reloadAll(parnetCode) {
      this.$refs.sortTree.getSorts();
      this.sortChange(parnetCode);
    }
  },
  mounted() {
    this.setPageData();
  }
};
</script>