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
        <h3>权限</h3>
        <div style="max-height:99%;overflow:auto;">
          <function-tree ref="functionTree" :parent="this"></function-tree>
        </div>

      </div>
      <div slot="right" class="demo-split-pane" style="max-height:100%;overflow:auto;">
        <h3>同级及下级</h3>
        <div>
          <Button
            v-if="userAccess.isAdd"
            class="search-btn"
            type="success"
            size="small"
            @click="handleAdd"
          >
            <Icon type="md-add" />&nbsp;&nbsp;新增
          </Button>
        </div>
        <div>
          <Table ref="tables" max-height ="700" :data="tableData1" v-bind:columns="tableColumns1" stripe>
                <template slot-scope="{ row, index }" slot="action">
                  <Button v-if="userAccess.isEdit" type="primary" size="small" icon="md-create" style="margin-right: 5px" @click="handleEdit(row)">编辑</Button>
                  <Button v-if="userAccess.isMove" type="error"  size="small" icon="md-trash" @click="handleDelete(row)">删除</Button>
               </template>
          </Table>
        </div>
      </div>
    </Split>
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
import FunctionTree from "./FunctionTree";
import Edit from "./Edit";
import { getChildList, remove } from "@/api/pubFunction";
import { pubFunction } from "@/access/pubFunction";
export default {
  data() {
    const userAccessAll = this.$store.state.user.access;
    return {
      userAccess: {
        isAdd: userAccessAll.includes(`${pubFunction.ADD}`),
        isEdit: userAccessAll.includes(`${pubFunction.EDIT}`),
        isMove: userAccessAll.includes(`${pubFunction.REMOVE}`),
        isAuth: userAccessAll.includes(`${pubFunction.AUTH}`)
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
          key: "functionCode"
        },
        {
          title: "中文名",
          key: "functionChina"
        },
        {
          title: "英文名",
          key: "functionEnglish"
        },
        {
          title: "修改时间",
          key: "editdate"
        },
        {
          title: "修改人",
          key: "editor"
        },
        {
          title: "操作",
          slot: "action",
          width: 300,
          align: "center"
        }
      ]
    };
  },
  components: {
    FunctionTree,
    Edit
  },
  methods: {
    setPageData() {
      getChildList(this.selectedCode)
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
        })
        .catch(err => {});
    },
    handleDelete(row) {
      this.$Modal.confirm({
        title: "提示",
        content: "<p>确定要删除[" + row.functionCode + "]?</p>",
        onOk: () => {
          // this.$Message.info("Clicked ok");
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
      var id = row.functionCode;
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
        .catch(err => {});
    },
    saveEdit() {
      var row = this.$refs.edit.Row;
    },
    functionChange(code) {
      this.selectedCode = code;
      this.setPageData();
    },
    reloadAll(parnetCode) {
      this.$refs.functionTree.getFunctions();
      this.functionChange(parnetCode);
    }
  },
  mounted() {
    this.setPageData();
  }
};
</script>