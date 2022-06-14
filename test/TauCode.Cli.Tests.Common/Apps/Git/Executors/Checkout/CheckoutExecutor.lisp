(executor

    (alternatives
        ;;; alternative: git checkout -b my-branch --track remote/my-branch
        (sequence
            (optional
                :is-entrance t

                (alternatives
                    (switch
                        :keys ("-b")
                        :alias "new-branch"
                    )
                    (switch
                        :keys ("-B")
                        :alias "new-branch-reset"
                    )
                )
            )
            (argument
                :alias "branch-name"
                :token-types ("branch-name")
            )
            (optional
                :is-exit t

                (key-value
                    :keys ("--track")
                    :alias "remote-branch-name"
                    :token-types ("branch-name")
                )
            )
        )
        ;;; alternative: git checkout 60d9d7c src/file.cpp
        (sequence
            (argument
                :alias "ref-name"
                :token-types ("ref-name")
                :is-entrance t
            )
            (argument
                :alias "file-path"
                :token-types ("file-path")
                :is-exit t
            )
        )
    )

    (end)
)
